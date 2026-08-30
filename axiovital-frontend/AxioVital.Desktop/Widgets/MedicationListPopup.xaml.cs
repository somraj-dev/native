using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;

namespace AxioVital.Desktop.Widgets;

public sealed partial class MedicationListPopup : UserControl
{
    public event EventHandler? CloseRequested;
    public event EventHandler? ReconcileAndSignCompleted;

    private enum ResizeDirection
    {
        None,
        Left,
        Right,
        Top,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    private ResizeDirection _currentResize = ResizeDirection.None;
    private Point _startPointerPos;
    private double _startWidth;
    private double _startHeight;
    private double _startTranslateX;
    private double _startTranslateY;

    private bool _isDraggingTitleBar = false;
    private Point _dragStartPointerPos;
    private double _dragStartTranslateX;
    private double _dragStartTranslateY;

    private bool _isMaximized = false;
    private double _restoreWidth = 1180;
    private double _restoreHeight = 740;
    private double _restoreTranslateX = 0;
    private double _restoreTranslateY = 0;

    public MedicationListPopup()
    {
        this.InitializeComponent();

        this.Loaded += (s, e) =>
        {
            SetupCursors();
            CenterDialogOnLoad();
        };

        this.SizeChanged += (s, e) =>
        {
            if (!_isMaximized && DialogBorder != null && RootLayout != null)
            {
                ClampDialogWithinBounds();
            }
        };
    }

    private void SetupCursors()
    {
        AttachCursor(ResizeTop, Microsoft.UI.Input.InputSystemCursorShape.SizeNorthSouth);
        AttachCursor(ResizeBottom, Microsoft.UI.Input.InputSystemCursorShape.SizeNorthSouth);
        AttachCursor(ResizeLeft, Microsoft.UI.Input.InputSystemCursorShape.SizeWestEast);
        AttachCursor(ResizeRight, Microsoft.UI.Input.InputSystemCursorShape.SizeWestEast);

        AttachCursor(ResizeTopLeft, Microsoft.UI.Input.InputSystemCursorShape.SizeNorthwestSoutheast);
        AttachCursor(ResizeBottomRight, Microsoft.UI.Input.InputSystemCursorShape.SizeNorthwestSoutheast);
        AttachCursor(ResizeTopRight, Microsoft.UI.Input.InputSystemCursorShape.SizeNortheastSouthwest);
        AttachCursor(ResizeBottomLeft, Microsoft.UI.Input.InputSystemCursorShape.SizeNortheastSouthwest);
    }

    private void AttachCursor(Border handle, Microsoft.UI.Input.InputSystemCursorShape shape)
    {
        if (handle == null) return;

        handle.PointerEntered += (s, e) =>
        {
            try
            {
                this.ProtectedCursor = Microsoft.UI.Input.InputSystemCursor.Create(shape);
            }
            catch { }
        };

        handle.PointerExited += (s, e) =>
        {
            if (_currentResize == ResizeDirection.None)
            {
                try
                {
                    this.ProtectedCursor = null;
                }
                catch { }
            }
        };
    }

    private void CenterDialogOnLoad()
    {
        if (_isMaximized || DialogBorder == null || RootLayout == null || DialogTranslate == null) return;

        double availableW = RootLayout.ActualWidth > 0 ? RootLayout.ActualWidth : 1300;
        double availableH = RootLayout.ActualHeight > 0 ? RootLayout.ActualHeight : 800;

        double targetW = Math.Min(1180, Math.Max(640, availableW * 0.85));
        double targetH = Math.Min(740, Math.Max(420, availableH * 0.86));

        DialogBorder.Width = targetW;
        DialogBorder.Height = targetH;
        DialogTranslate.X = Math.Max(12, (availableW - targetW) / 2.0);
        DialogTranslate.Y = Math.Max(12, (availableH - targetH) / 2.0);
    }

    private void ClampDialogWithinBounds()
    {
        if (RootLayout == null || DialogBorder == null || DialogTranslate == null) return;

        double rootW = RootLayout.ActualWidth;
        double rootH = RootLayout.ActualHeight;
        if (rootW <= 0 || rootH <= 0) return;

        if (DialogBorder.Width > rootW) DialogBorder.Width = Math.Max(DialogBorder.MinWidth, rootW - 20);
        if (DialogBorder.Height > rootH) DialogBorder.Height = Math.Max(DialogBorder.MinHeight, rootH - 20);

        if (DialogTranslate.X + DialogBorder.Width > rootW)
            DialogTranslate.X = Math.Max(0, rootW - DialogBorder.Width - 10);
        if (DialogTranslate.Y + DialogBorder.Height > rootH)
            DialogTranslate.Y = Math.Max(0, rootH - DialogBorder.Height - 10);
    }

    #region Dragging Title Bar
    private void OnTitleBarPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (_isMaximized) return;
        _isDraggingTitleBar = true;
        _dragStartPointerPos = e.GetCurrentPoint(RootLayout).Position;
        _dragStartTranslateX = DialogTranslate.X;
        _dragStartTranslateY = DialogTranslate.Y;
        ((FrameworkElement)sender).CapturePointer(e.Pointer);
        e.Handled = true;
    }

    private void OnTitleBarPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (!_isDraggingTitleBar) return;
        var curPos = e.GetCurrentPoint(RootLayout).Position;
        double deltaX = curPos.X - _dragStartPointerPos.X;
        double deltaY = curPos.Y - _dragStartPointerPos.Y;

        DialogTranslate.X = _dragStartTranslateX + deltaX;
        DialogTranslate.Y = _dragStartTranslateY + deltaY;
        e.Handled = true;
    }

    private void OnTitleBarPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (_isDraggingTitleBar)
        {
            _isDraggingTitleBar = false;
            ((FrameworkElement)sender).ReleasePointerCapture(e.Pointer);
            e.Handled = true;
        }
    }

    private void OnTitleBarDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        ToggleMaximize();
        e.Handled = true;
    }
    #endregion

    #region Maximize and Restore
    private void OnMaximizeClicked(object sender, RoutedEventArgs e)
    {
        ToggleMaximize();
    }

    private void ToggleMaximize()
    {
        if (DialogBorder == null || DialogTranslate == null) return;

        if (_isMaximized)
        {
            // Restore to floating size
            _isMaximized = false;
            DialogBorder.HorizontalAlignment = HorizontalAlignment.Left;
            DialogBorder.VerticalAlignment = VerticalAlignment.Top;
            DialogBorder.Width = _restoreWidth;
            DialogBorder.Height = _restoreHeight;
            DialogTranslate.X = _restoreTranslateX;
            DialogTranslate.Y = _restoreTranslateY;
            if (MaximizeButtonText != null) MaximizeButtonText.Text = "□";
        }
        else
        {
            // Maximize to fill root container
            _isMaximized = true;
            _restoreWidth = DialogBorder.ActualWidth > 0 ? DialogBorder.ActualWidth : DialogBorder.Width;
            _restoreHeight = DialogBorder.ActualHeight > 0 ? DialogBorder.ActualHeight : DialogBorder.Height;
            _restoreTranslateX = DialogTranslate.X;
            _restoreTranslateY = DialogTranslate.Y;

            DialogTranslate.X = 0;
            DialogTranslate.Y = 0;
            DialogBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
            DialogBorder.VerticalAlignment = VerticalAlignment.Stretch;
            DialogBorder.Width = double.NaN;
            DialogBorder.Height = double.NaN;
            DialogBorder.Margin = new Thickness(0);
            if (MaximizeButtonText != null) MaximizeButtonText.Text = "❐";
        }
    }
    #endregion

    #region Resizing Handles
    private void StartResize(ResizeDirection direction, FrameworkElement handle, PointerRoutedEventArgs e)
    {
        if (_isMaximized || DialogBorder == null || DialogTranslate == null) return;
        _currentResize = direction;
        _startPointerPos = e.GetCurrentPoint(RootLayout).Position;
        _startWidth = DialogBorder.ActualWidth > 0 ? DialogBorder.ActualWidth : DialogBorder.Width;
        _startHeight = DialogBorder.ActualHeight > 0 ? DialogBorder.ActualHeight : DialogBorder.Height;
        _startTranslateX = DialogTranslate.X;
        _startTranslateY = DialogTranslate.Y;
        handle.CapturePointer(e.Pointer);
        e.Handled = true;
    }

    private void OnResizeTopPressed(object sender, PointerRoutedEventArgs e) => StartResize(ResizeDirection.Top, (FrameworkElement)sender, e);
    private void OnResizeBottomPressed(object sender, PointerRoutedEventArgs e) => StartResize(ResizeDirection.Bottom, (FrameworkElement)sender, e);
    private void OnResizeLeftPressed(object sender, PointerRoutedEventArgs e) => StartResize(ResizeDirection.Left, (FrameworkElement)sender, e);
    private void OnResizeRightPressed(object sender, PointerRoutedEventArgs e) => StartResize(ResizeDirection.Right, (FrameworkElement)sender, e);

    private void OnResizeTopLeftPressed(object sender, PointerRoutedEventArgs e) => StartResize(ResizeDirection.TopLeft, (FrameworkElement)sender, e);
    private void OnResizeTopRightPressed(object sender, PointerRoutedEventArgs e) => StartResize(ResizeDirection.TopRight, (FrameworkElement)sender, e);
    private void OnResizeBottomLeftPressed(object sender, PointerRoutedEventArgs e) => StartResize(ResizeDirection.BottomLeft, (FrameworkElement)sender, e);
    private void OnResizeBottomRightPressed(object sender, PointerRoutedEventArgs e) => StartResize(ResizeDirection.BottomRight, (FrameworkElement)sender, e);

    private void OnResizePointerMoved(object sender, PointerRoutedEventArgs e)
    {
        if (_currentResize == ResizeDirection.None || DialogBorder == null || DialogTranslate == null || RootLayout == null) return;

        var curPos = e.GetCurrentPoint(RootLayout).Position;
        double deltaX = curPos.X - _startPointerPos.X;
        double deltaY = curPos.Y - _startPointerPos.Y;

        double minW = DialogBorder.MinWidth > 0 ? DialogBorder.MinWidth : 600;
        double minH = DialogBorder.MinHeight > 0 ? DialogBorder.MinHeight : 380;
        double maxW = RootLayout.ActualWidth > 0 ? RootLayout.ActualWidth : 3000;
        double maxH = RootLayout.ActualHeight > 0 ? RootLayout.ActualHeight : 2000;

        switch (_currentResize)
        {
            case ResizeDirection.Right:
                DialogBorder.Width = Math.Min(maxW, Math.Max(minW, _startWidth + deltaX));
                break;

            case ResizeDirection.Bottom:
                DialogBorder.Height = Math.Min(maxH, Math.Max(minH, _startHeight + deltaY));
                break;

            case ResizeDirection.Left:
                {
                    double newW = Math.Min(maxW, Math.Max(minW, _startWidth - deltaX));
                    double actualDeltaX = _startWidth - newW;
                    DialogBorder.Width = newW;
                    DialogTranslate.X = _startTranslateX + actualDeltaX;
                }
                break;

            case ResizeDirection.Top:
                {
                    double newH = Math.Min(maxH, Math.Max(minH, _startHeight - deltaY));
                    double actualDeltaY = _startHeight - newH;
                    DialogBorder.Height = newH;
                    DialogTranslate.Y = _startTranslateY + actualDeltaY;
                }
                break;

            case ResizeDirection.BottomRight:
                DialogBorder.Width = Math.Min(maxW, Math.Max(minW, _startWidth + deltaX));
                DialogBorder.Height = Math.Min(maxH, Math.Max(minH, _startHeight + deltaY));
                break;

            case ResizeDirection.BottomLeft:
                {
                    double newW = Math.Min(maxW, Math.Max(minW, _startWidth - deltaX));
                    double actualDeltaX = _startWidth - newW;
                    DialogBorder.Width = newW;
                    DialogTranslate.X = _startTranslateX + actualDeltaX;
                    DialogBorder.Height = Math.Min(maxH, Math.Max(minH, _startHeight + deltaY));
                }
                break;

            case ResizeDirection.TopRight:
                {
                    DialogBorder.Width = Math.Min(maxW, Math.Max(minW, _startWidth + deltaX));
                    double newH = Math.Min(maxH, Math.Max(minH, _startHeight - deltaY));
                    double actualDeltaY = _startHeight - newH;
                    DialogBorder.Height = newH;
                    DialogTranslate.Y = _startTranslateY + actualDeltaY;
                }
                break;

            case ResizeDirection.TopLeft:
                {
                    double newW = Math.Min(maxW, Math.Max(minW, _startWidth - deltaX));
                    double actualDeltaX = _startWidth - newW;
                    DialogBorder.Width = newW;
                    DialogTranslate.X = _startTranslateX + actualDeltaX;

                    double newH = Math.Min(maxH, Math.Max(minH, _startHeight - deltaY));
                    double actualDeltaY = _startHeight - newH;
                    DialogBorder.Height = newH;
                    DialogTranslate.Y = _startTranslateY + actualDeltaY;
                }
                break;
        }

        e.Handled = true;
    }

    private void OnResizePointerReleased(object sender, PointerRoutedEventArgs e)
    {
        if (_currentResize != ResizeDirection.None)
        {
            _currentResize = ResizeDirection.None;
            ((FrameworkElement)sender).ReleasePointerCapture(e.Pointer);
            try
            {
                this.ProtectedCursor = null;
            }
            catch { }
            e.Handled = true;
        }
    }
    #endregion

    #region Action Button Handlers
    private void OnCloseClicked(object sender, RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnReconcileAndSignClicked(object sender, RoutedEventArgs e)
    {
        ReconcileAndSignCompleted?.Invoke(this, EventArgs.Empty);
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnAddClicked(object sender, RoutedEventArgs e)
    {
        // Add new medication order interaction hook
    }
    #endregion
}
