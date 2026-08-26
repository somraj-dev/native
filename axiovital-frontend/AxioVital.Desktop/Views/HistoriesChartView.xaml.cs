using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;

namespace AxioVital.Desktop.Views;

public sealed partial class HistoriesChartView : UserControl
{
    public HistoriesChartView()
    {
        this.InitializeComponent();
        PopulateHeatmapRows();
    }

    private void PopulateHeatmapRows()
    {
        if (HeatmapDataGrid == null) return;
        HeatmapDataGrid.Children.Clear();

        var rows = GetHistoricalCalendarData();

        for (int i = 0; i < rows.Count; i++)
        {
            var r = rows[i];
            var grid = new Grid
            {
                Height = 17,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(45) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(140) });
            for (int d = 0; d < 7; d++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star), MinWidth = 60 });
            }
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.8, GridUnitType.Star), MinWidth = 140 });

            // 1. Month Label (Dec, Jan, Feb, etc.)
            if (!string.IsNullOrEmpty(r.MonthName))
            {
                var monthTb = new TextBlock
                {
                    Text = r.MonthName,
                    FontSize = 9.5,
                    FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 6, 0)
                };
                Grid.SetColumn(monthTb, 0);
                grid.Children.Add(monthTb);
            }

            // 2. Mini Calendar Block (7 days representation)
            var miniGrid = new Grid { HorizontalAlignment = HorizontalAlignment.Stretch };
            for (int md = 0; md < 7; md++) miniGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            for (int md = 0; md < 7; md++)
            {
                if (r.MiniCalendarDays[md] > 0)
                {
                    var dayBorder = new Border
                    {
                        BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 208, 208, 208)),
                        BorderThickness = new Thickness(0.5),
                        Background = r.IsMiniCalendarHighlight[md] ? new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 185, 205, 229)) : new SolidColorBrush(Microsoft.UI.Colors.Transparent)
                    };
                    var dayTb = new TextBlock
                    {
                        Text = r.MiniCalendarDays[md].ToString(),
                        FontSize = 7.5,
                        Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    dayBorder.Child = dayTb;
                    Grid.SetColumn(dayBorder, md);
                    miniGrid.Children.Add(dayBorder);
                }
            }
            Grid.SetColumn(miniGrid, 1);
            grid.Children.Add(miniGrid);

            // 3. 7 Day Data Heatmap Cells (Sun - Sat)
            for (int d = 0; d < 7; d++)
            {
                var cellBorder = new Border
                {
                    BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 208, 208, 208)),
                    BorderThickness = new Thickness(0.5),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };

                if (r.DayValues[d].HasValue)
                {
                    int val = r.DayValues[d]!.Value;
                    cellBorder.Background = GetExactExcelGradientBrush(val);

                    var cellTb = new TextBlock
                    {
                        Text = val.ToString(),
                        FontSize = 9,
                        Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    cellBorder.Child = cellTb;
                }
                else
                {
                    // Empty date placeholder cell
                    cellBorder.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
                    cellBorder.Child = new Border
                    {
                        Height = 1,
                        Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 220, 220, 220)),
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(8, 0, 8, 0)
                    };
                }

                Grid.SetColumn(cellBorder, d + 2);
                grid.Children.Add(cellBorder);
            }

            // 4. Weekly Average Column (Numeric text + Responsive Proportional Soft Blue Data Bar)
            var avgGrid = new Grid
            {
                BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 208, 208, 208)),
                BorderThickness = new Thickness(0.5),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(Microsoft.UI.Colors.White)
            };
            avgGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(45) });
            avgGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var avgTb = new TextBlock
            {
                Text = r.WeeklyAverage.ToString(),
                FontSize = 9,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetColumn(avgTb, 0);
            avgGrid.Children.Add(avgTb);

            // Responsive Data Bar
            double maxVal = 7500.0;
            double pct = Math.Min(1.0, Math.Max(0.01, (double)r.WeeklyAverage / maxVal));

            var barContainer = new Grid
            {
                Height = 11,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(4, 0, 6, 0)
            };
            barContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(pct, GridUnitType.Star) });
            barContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(Math.Max(0.001, 1.0 - pct), GridUnitType.Star) });

            var barBorder = new Border
            {
                Height = 11,
                Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 149, 179, 215)),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            Grid.SetColumn(barBorder, 0);
            barContainer.Children.Add(barBorder);

            Grid.SetColumn(barContainer, 1);
            avgGrid.Children.Add(barContainer);

            Grid.SetColumn(avgGrid, 9);
            grid.Children.Add(avgGrid);

            HeatmapDataGrid.Children.Add(grid);
        }
    }

    private static Brush GetExactExcelGradientBrush(int val)
    {
        // Exact Excel 3-Color Scale from the reference image:
        // Min ~900-1000 : Soft Green (99, 190, 123) #63BE7B
        // Mid ~4000     : Light Yellow (255, 235, 132) #FFEB84
        // Max ~7500+    : Soft Coral Red (248, 105, 107) #F8696B

        byte r, g, b;
        if (val <= 4000)
        {
            double t = Math.Max(0.0, Math.Min(1.0, (val - 1000.0) / 3000.0));
            r = (byte)(99 + (255 - 99) * t);
            g = (byte)(190 + (235 - 190) * t);
            b = (byte)(123 + (132 - 123) * t);
        }
        else
        {
            double t = Math.Max(0.0, Math.Min(1.0, (val - 4000.0) / 3500.0));
            r = (byte)(255 + (248 - 255) * t);
            g = (byte)(235 + (105 - 235) * t);
            b = (byte)(132 + (107 - 132) * t);
        }

        return new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, r, g, b));
    }

    private class CalendarRowData
    {
        public string MonthName { get; set; } = "";
        public int[] MiniCalendarDays { get; set; } = new int[7];
        public bool[] IsMiniCalendarHighlight { get; set; } = new bool[7];
        public int?[] DayValues { get; set; } = new int?[7];
        public int WeeklyAverage { get; set; }
    }

    private List<CalendarRowData> GetHistoricalCalendarData()
    {
        return new List<CalendarRowData>
        {
            // Dec 2012 / Jan 2013
            new CalendarRowData { MonthName = "Dec", MiniCalendarDays = new int[] { 30, 31, 1, 2, 3, 4, 5 }, IsMiniCalendarHighlight = new bool[] { true, true, false, false, false, false, false }, DayValues = new int?[] { null, null, 5576, 12259, 5857, 6141, 3855 }, WeeklyAverage = 7344 },
            new CalendarRowData { MonthName = "Jan", MiniCalendarDays = new int[] { 6, 7, 8, 9, 10, 11, 12 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 4490, 7504, 6766, 5695, 5164, 4433, 2866 }, WeeklyAverage = 6274 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 13, 14, 15, 16, 17, 18, 19 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 3593, 5810, 5375, 5791, 5457, 4280, 3047 }, WeeklyAverage = 4836 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 20, 21, 22, 23, 24, 25, 26 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 3439, 4705, 5539, 5754, 5761, 4537, 3343 }, WeeklyAverage = 4726 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 27, 28, 29, 30, 31, 1, 2 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, true, true }, DayValues = new int?[] { 4234, 6363, 6570, 6679, 6683, 5523, 3659 }, WeeklyAverage = 5716 },
            
            // Feb
            new CalendarRowData { MonthName = "Feb", MiniCalendarDays = new int[] { 3, 4, 5, 6, 7, 8, 9 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 3856, 6924, 5996, 5873, 5622, 4148, 2673 }, WeeklyAverage = 5013 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 10, 11, 12, 13, 14, 15, 16 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 3507, 5351, 5120, 4984, 4340, 4240, 2945 }, WeeklyAverage = 4360 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 17, 18, 19, 20, 21, 22, 23 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 3538, 5206, 5870, 5384, 5183, 4349, 3148 }, WeeklyAverage = 4670 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 24, 25, 26, 27, 28, 1, 2 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, true, true }, DayValues = new int?[] { 3781, 6174, 6579, 6378, 6049, 4953, 3580 }, WeeklyAverage = 5356 },

            // Mar
            new CalendarRowData { MonthName = "Mar", MiniCalendarDays = new int[] { 3, 4, 5, 6, 7, 8, 9 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 4042, 6260, 5877, 5828, 5445, 4578, 3042 }, WeeklyAverage = 5011 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 10, 11, 12, 13, 14, 15, 16 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 3633, 5860, 5776, 5270, 5070, 4070, 2886 }, WeeklyAverage = 4652 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 17, 18, 19, 20, 21, 22, 23 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 4527, 5387, 4942, 5062, 4978, 3818, 2802 }, WeeklyAverage = 4502 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 24, 25, 26, 27, 28, 29, 30 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 3301, 5473, 4830, 4654, 4284, 3492, 2330 }, WeeklyAverage = 4052 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 31, 1, 2, 3, 4, 5, 6 }, IsMiniCalendarHighlight = new bool[] { false, true, true, true, true, true, true }, DayValues = new int?[] { 2445, 4630, 5157, 5138, 4894, 3952, 2702 }, WeeklyAverage = 4160 },

            // Apr
            new CalendarRowData { MonthName = "Apr", MiniCalendarDays = new int[] { 7, 8, 9, 10, 11, 12, 13 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 3097, 5186, 4991, 4973, 4761, 4080, 2752 }, WeeklyAverage = 4263 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 14, 15, 16, 17, 18, 19, 20 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 3317, 5057, 5122, 4907, 4683, 3904, 2616 }, WeeklyAverage = 4229 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 21, 22, 23, 24, 25, 26, 27 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 3169, 5074, 5160, 4933, 4552, 3762, 2289 }, WeeklyAverage = 4134 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 28, 29, 30, 1, 2, 3, 4 }, IsMiniCalendarHighlight = new bool[] { false, false, false, true, true, true, true }, DayValues = new int?[] { 2948, 4975, 4852, 4405, 4420, 3535, 2168 }, WeeklyAverage = 3900 },

            // May
            new CalendarRowData { MonthName = "May", MiniCalendarDays = new int[] { 5, 6, 7, 8, 9, 10, 11 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 2510, 4356, 4214, 3855, 3377, 2796, 1629 }, WeeklyAverage = 3248 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 12, 13, 14, 15, 16, 17, 18 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1889, 3782, 3648, 3570, 3429, 2611, 1726 }, WeeklyAverage = 3008 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 19, 20, 21, 22, 23, 24, 25 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 2161, 3617, 3808, 3602, 3466, 2861, 1663 }, WeeklyAverage = 3025 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 26, 27, 28, 29, 30, 31, 1 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, true }, DayValues = new int?[] { 1831, 2600, 4004, 3917, 3411, 2859, 1711 }, WeeklyAverage = 2905 },

            // Jun
            new CalendarRowData { MonthName = "Jun", MiniCalendarDays = new int[] { 2, 3, 4, 5, 6, 7, 8 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 2166, 3394, 3495, 3414, 3242, 2649, 1554 }, WeeklyAverage = 2845 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 9, 10, 11, 12, 13, 14, 15 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1961, 3432, 3456, 3223, 3231, 2429, 1489 }, WeeklyAverage = 2746 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 16, 17, 18, 19, 20, 21, 22 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1549, 3218, 3307, 3113, 2829, 2385, 1517 }, WeeklyAverage = 2560 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 23, 24, 25, 26, 27, 28, 29 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1907, 3154, 3211, 3027, 2904, 2294, 1436 }, WeeklyAverage = 2562 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 30, 1, 2, 3, 4, 5, 6 }, IsMiniCalendarHighlight = new bool[] { false, true, true, true, true, true, true }, DayValues = new int?[] { 1820, 3228, 2926, 2391, 1593, 1848, 1261 }, WeeklyAverage = 2155 },

            // Jul
            new CalendarRowData { MonthName = "Jul", MiniCalendarDays = new int[] { 7, 8, 9, 10, 11, 12, 13 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1639, 2771, 3326, 3058, 2317, 2228, 1423 }, WeeklyAverage = 2395 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 14, 15, 16, 17, 18, 19, 20 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1701, 3165, 3207, 3212, 3081, 2629, 1483 }, WeeklyAverage = 2640 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 21, 22, 23, 24, 25, 26, 27 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1845, 3270, 3465, 3302, 2947, 2444, 1415 }, WeeklyAverage = 2670 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 28, 29, 30, 31, 1, 2, 3 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, true, true, true }, DayValues = new int?[] { 1767, 3276, 3305, 3340, 3251, 2597, 1475 }, WeeklyAverage = 2716 },

            // Aug
            new CalendarRowData { MonthName = "Aug", MiniCalendarDays = new int[] { 4, 5, 6, 7, 8, 9, 10 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1763, 3176, 3361, 3268, 3011, 2231, 1416 }, WeeklyAverage = 2604 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 11, 12, 13, 14, 15, 16, 17 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1790, 3390, 3320, 3278, 3332, 2700, 1629 }, WeeklyAverage = 2777 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 18, 19, 20, 21, 22, 23, 24 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 2033, 3781, 3759, 3596, 3468, 3003, 1844 }, WeeklyAverage = 3069 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 25, 26, 27, 28, 29, 30, 31 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 2378, 4094, 4252, 4153, 4055, 3060, 1727 }, WeeklyAverage = 3388 },

            // Sep
            new CalendarRowData { MonthName = "Sep", MiniCalendarDays = new int[] { 1, 2, 3, 4, 5, 6, 7 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1935, 3050, 4371, 4366, 4138, 3213, 1785 }, WeeklyAverage = 3266 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 8, 9, 10, 11, 12, 13, 14 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 2158, 3933, 3782, 3587, 3285, 2448, 1435 }, WeeklyAverage = 2947 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 15, 16, 17, 18, 19, 20, 21 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1871, 3174, 3006, 3010, 2941, 2626, 1637 }, WeeklyAverage = 2609 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 22, 23, 24, 25, 26, 27, 28 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 2005, 3532, 3577, 3752, 3837, 3385, 1866 }, WeeklyAverage = 3137 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 29, 30, 1, 2, 3, 4, 5 }, IsMiniCalendarHighlight = new bool[] { false, false, true, true, true, true, true }, DayValues = new int?[] { 2447, 4454, 4450, 4419, 4089, 3346, 1795 }, WeeklyAverage = 3571 },

            // Oct
            new CalendarRowData { MonthName = "Oct", MiniCalendarDays = new int[] { 6, 7, 8, 9, 10, 11, 12 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 2222, 4268, 4433, 3848, 3658, 2885, 1499 }, WeeklyAverage = 3259 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 13, 14, 15, 16, 17, 18, 19 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1924, 3402, 3426, 3425, 3306, 2613, 1489 }, WeeklyAverage = 2826 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 20, 21, 22, 23, 24, 25, 26 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1857, 3738, 3524, 3622, 3359, 2864, 1624 }, WeeklyAverage = 2941 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 27, 28, 29, 30, 31, 1, 2 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, true, true }, DayValues = new int?[] { 2065, 3673, 4167, 4493, 3202, 2888, 1571 }, WeeklyAverage = 3151 },

            // Nov
            new CalendarRowData { MonthName = "Nov", MiniCalendarDays = new int[] { 3, 4, 5, 6, 7, 8, 9 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1898, 3453, 3497, 3282, 3074, 2450, 1486 }, WeeklyAverage = 2734 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 10, 11, 12, 13, 14, 15, 16 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1738, 2986, 3251, 3265, 3089, 2527, 1458 }, WeeklyAverage = 2616 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 17, 18, 19, 20, 21, 22, 23 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1849, 3248, 3146, 3111, 2989, 2349, 1402 }, WeeklyAverage = 2565 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 24, 25, 26, 27, 28, 29, 30 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1679, 3028, 2949, 2200, 1351, 1639, 1433 }, WeeklyAverage = 2040 },

            // Dec 2013 / Jan 2014
            new CalendarRowData { MonthName = "Dec", MiniCalendarDays = new int[] { 1, 2, 3, 4, 5, 6, 7 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1933, 3689, 3407, 3144, 3071, 2582, 1674 }, WeeklyAverage = 2786 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 8, 9, 10, 11, 12, 13, 14 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1845, 3527, 3160, 3159, 3040, 2382, 1456 }, WeeklyAverage = 2653 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 15, 16, 17, 18, 19, 20, 21 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1637, 3312, 2925, 2996, 2697, 2211, 1270 }, WeeklyAverage = 2435 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 22, 23, 24, 25, 26, 27, 28 }, IsMiniCalendarHighlight = new bool[] { false, false, false, false, false, false, false }, DayValues = new int?[] { 1503, 2304, 1480, 903, 2526, 2776, 1959 }, WeeklyAverage = 1922 },
            new CalendarRowData { MonthName = "", MiniCalendarDays = new int[] { 29, 30, 31, 1, 2, 3, 4 }, IsMiniCalendarHighlight = new bool[] { false, false, false, true, true, true, true }, DayValues = new int?[] { 2277, 4123, 3473, 3343, 5472, 3805, 2181 }, WeeklyAverage = 3668 }
        };
    }
}
