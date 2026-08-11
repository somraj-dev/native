using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AxioVitalSetup;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new InstallerForm());
    }
}

public class InstallerForm : Form
{
    private Panel leftPanel = null!;
    private Panel bottomPanel = null!;
    private Panel mainPanel = null!;

    private Button btnAbout = null!;
    private Button btnBack = null!;
    private Button btnNext = null!;
    private Button btnCancel = null!;

    // Page controls
    private Panel pnlWelcome = null!;
    private Panel pnlSelectFolder = null!;
    private Panel pnlInstalling = null!;
    private Panel pnlFinish = null!;

    // Select Folder controls
    private TextBox txtFolderPath = null!;
    private CheckBox chkDesktopShortcut = null!;
    private CheckBox chkStartMenuShortcut = null!;

    // Installing controls
    private ProgressBar progressBar = null!;
    private Label lblStatus = null!;

    // Finish controls
    private CheckBox chkLaunchApp = null!;

    private int currentStep = 0;
    private string installPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs", "AxioVital");

    public InstallerForm()
    {
        InitializeComponents();
        ShowStep(0);
    }

    private void InitializeComponents()
    {
        this.Text = "AxioVital 1.0 Setup";
        this.Size = new Size(570, 420);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = true;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        this.BackColor = Color.White;

        // Bottom Panel
        bottomPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 52,
            BackColor = Color.FromArgb(240, 240, 240)
        };
        bottomPanel.Paint += (s, e) =>
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(210, 210, 210)), 0, 0, bottomPanel.Width, 0);
        };

        btnAbout = new Button { Text = "About", Location = new Point(14, 12), Size = new Size(82, 26) };
        btnBack = new Button { Text = "< Back", Location = new Point(300, 12), Size = new Size(82, 26) };
        btnNext = new Button { Text = "Next >", Location = new Point(388, 12), Size = new Size(82, 26) };
        btnCancel = new Button { Text = "Cancel", Location = new Point(476, 12), Size = new Size(82, 26) };

        btnAbout.Click += (s, e) => MessageBox.Show("AxioVital Native Desktop Application Installer 1.0\n© 2026 AxioVital Corporation. All rights reserved.", "About AxioVital Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
        btnBack.Click += (s, e) => ShowStep(currentStep - 1);
        btnNext.Click += OnNextClicked;
        btnCancel.Click += (s, e) => this.Close();

        bottomPanel.Controls.Add(btnAbout);
        bottomPanel.Controls.Add(btnBack);
        bottomPanel.Controls.Add(btnNext);
        bottomPanel.Controls.Add(btnCancel);

        // Left Panel (Gradient Banner)
        leftPanel = new Panel
        {
            Dock = DockStyle.Left,
            Width = 175
        };
        leftPanel.Paint += (s, e) =>
        {
            using var brush = new LinearGradientBrush(leftPanel.ClientRectangle, Color.FromArgb(0, 92, 138), Color.FromArgb(10, 63, 92), 60f);
            e.Graphics.FillRectangle(brush, leftPanel.ClientRectangle);

            using var titleFont = new Font("Segoe UI", 12F, FontStyle.Bold);
            using var subFont = new Font("Segoe UI", 9F, FontStyle.Italic);
            using var textBrush = new SolidBrush(Color.White);
            using var subBrush = new SolidBrush(Color.FromArgb(200, 225, 240));

            e.Graphics.DrawString("AxioVital", titleFont, textBrush, new PointF(16, 24));
            e.Graphics.DrawString("Environment ™", subFont, subBrush, new PointF(16, 48));
            e.Graphics.DrawString("Native EHR Desktop", subFont, subBrush, new PointF(16, 68));
        };

        // Main Content Panel
        mainPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20)
        };

        CreateWelcomePage();
        CreateSelectFolderPage();
        CreateInstallingPage();
        CreateFinishPage();

        this.Controls.Add(mainPanel);
        this.Controls.Add(leftPanel);
        this.Controls.Add(bottomPanel);
    }

    private void CreateWelcomePage()
    {
        pnlWelcome = new Panel { Dock = DockStyle.Fill, Visible = false };

        var lblTitle = new Label
        {
            Text = "Welcome to the AxioVital Setup Wizard !",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = Color.FromArgb(10, 63, 92),
            Location = new Point(10, 15),
            AutoSize = true
        };

        var lblDesc = new Label
        {
            Text = "This wizard will guide you through the installation of AxioVital Native Desktop EHR Application on your computer.\n\nIt is recommended that you close all other applications before continuing.",
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(40, 40, 40),
            Location = new Point(10, 65),
            Size = new Size(350, 100)
        };

        var lblLink = new Label
        {
            Text = "Visit our website for latest releases and downloads:\nhttp://www.AxioVital.com",
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(0, 92, 138),
            Location = new Point(10, 180),
            Size = new Size(350, 40)
        };

        var lblPrompt = new Label
        {
            Text = "Click 'Next' to continue...",
            Font = new Font("Segoe UI", 9F, FontStyle.Italic),
            ForeColor = Color.FromArgb(80, 80, 80),
            Location = new Point(10, 260),
            AutoSize = true
        };

        pnlWelcome.Controls.Add(lblTitle);
        pnlWelcome.Controls.Add(lblDesc);
        pnlWelcome.Controls.Add(lblLink);
        pnlWelcome.Controls.Add(lblPrompt);

        mainPanel.Controls.Add(pnlWelcome);
    }

    private void CreateSelectFolderPage()
    {
        pnlSelectFolder = new Panel { Dock = DockStyle.Fill, Visible = false };

        var lblTitle = new Label
        {
            Text = "Select Destination Location",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = Color.FromArgb(10, 63, 92),
            Location = new Point(10, 15),
            AutoSize = true
        };

        var lblDesc = new Label
        {
            Text = "Where should AxioVital Desktop be installed?",
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(40, 40, 40),
            Location = new Point(10, 50),
            AutoSize = true
        };

        txtFolderPath = new TextBox
        {
            Text = installPath,
            Location = new Point(10, 85),
            Size = new Size(260, 25)
        };

        var btnBrowse = new Button
        {
            Text = "Browse...",
            Location = new Point(278, 84),
            Size = new Size(75, 26)
        };
        btnBrowse.Click += (s, e) =>
        {
            using var dlg = new FolderBrowserDialog { SelectedPath = txtFolderPath.Text };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtFolderPath.Text = dlg.SelectedPath;
            }
        };

        chkDesktopShortcut = new CheckBox
        {
            Text = "Create a Desktop shortcut",
            Checked = true,
            Location = new Point(10, 140),
            AutoSize = true
        };

        chkStartMenuShortcut = new CheckBox
        {
            Text = "Create a Start Menu shortcut",
            Checked = true,
            Location = new Point(10, 170),
            AutoSize = true
        };

        pnlSelectFolder.Controls.Add(lblTitle);
        pnlSelectFolder.Controls.Add(lblDesc);
        pnlSelectFolder.Controls.Add(txtFolderPath);
        pnlSelectFolder.Controls.Add(btnBrowse);
        pnlSelectFolder.Controls.Add(chkDesktopShortcut);
        pnlSelectFolder.Controls.Add(chkStartMenuShortcut);

        mainPanel.Controls.Add(pnlSelectFolder);
    }

    private void CreateInstallingPage()
    {
        pnlInstalling = new Panel { Dock = DockStyle.Fill, Visible = false };

        var lblTitle = new Label
        {
            Text = "Installing AxioVital Desktop...",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = Color.FromArgb(10, 63, 92),
            Location = new Point(10, 15),
            AutoSize = true
        };

        lblStatus = new Label
        {
            Text = "Preparing installation...",
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(60, 60, 60),
            Location = new Point(10, 65),
            Size = new Size(340, 20)
        };

        progressBar = new ProgressBar
        {
            Location = new Point(10, 95),
            Size = new Size(345, 22),
            Minimum = 0,
            Maximum = 100,
            Value = 0
        };

        pnlInstalling.Controls.Add(lblTitle);
        pnlInstalling.Controls.Add(lblStatus);
        pnlInstalling.Controls.Add(progressBar);

        mainPanel.Controls.Add(pnlInstalling);
    }

    private void CreateFinishPage()
    {
        pnlFinish = new Panel { Dock = DockStyle.Fill, Visible = false };

        var lblTitle = new Label
        {
            Text = "Completing the AxioVital Setup Wizard",
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = Color.FromArgb(10, 63, 92),
            Location = new Point(10, 15),
            AutoSize = true
        };

        var lblDesc = new Label
        {
            Text = "AxioVital Desktop has been successfully installed on your computer.\n\nClick 'Finish' to exit Setup.",
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(40, 40, 40),
            Location = new Point(10, 65),
            Size = new Size(350, 80)
        };

        chkLaunchApp = new CheckBox
        {
            Text = "Launch AxioVital Desktop Environment now",
            Checked = true,
            Location = new Point(10, 160),
            AutoSize = true
        };

        pnlFinish.Controls.Add(lblTitle);
        pnlFinish.Controls.Add(lblDesc);
        pnlFinish.Controls.Add(chkLaunchApp);

        mainPanel.Controls.Add(pnlFinish);
    }

    private void ShowStep(int step)
    {
        currentStep = step;

        pnlWelcome.Visible = (step == 0);
        pnlSelectFolder.Visible = (step == 1);
        pnlInstalling.Visible = (step == 2);
        pnlFinish.Visible = (step == 3);

        btnBack.Enabled = (step == 1);
        btnNext.Enabled = (step != 2);
        btnCancel.Text = (step == 3) ? "Close" : "Cancel";

        if (step == 3)
        {
            btnNext.Text = "Finish";
        }
        else
        {
            btnNext.Text = "Next >";
        }
    }

    private async void OnNextClicked(object? sender, EventArgs e)
    {
        if (currentStep == 0)
        {
            ShowStep(1);
        }
        else if (currentStep == 1)
        {
            installPath = txtFolderPath.Text.Trim();
            ShowStep(2);
            await PerformInstallationAsync();
        }
        else if (currentStep == 3)
        {
            if (chkLaunchApp.Checked)
            {
                string exePath = Path.Combine(installPath, "AxioVital.Desktop.exe");
                if (File.Exists(exePath))
                {
                    Process.Start(new ProcessStartInfo { FileName = exePath, UseShellExecute = true });
                }
            }
            this.Close();
        }
    }

    private async Task PerformInstallationAsync()
    {
        try
        {
            lblStatus.Text = "Creating installation directory...";
            progressBar.Value = 10;
            await Task.Delay(300);

            Directory.CreateDirectory(installPath);

            lblStatus.Text = "Extracting AxioVital Desktop binaries...";
            progressBar.Value = 30;
            await Task.Delay(400);

            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("AxioVitalSetup.AxioVitalPayload.zip"))
            {
                if (stream != null)
                {
                    string tempZip = Path.Combine(Path.GetTempPath(), "AxioVitalPayload_" + Guid.NewGuid().ToString("N") + ".zip");
                    using (var fs = new FileStream(tempZip, FileMode.Create, FileAccess.Write))
                    {
                        await stream.CopyToAsync(fs);
                    }

                    progressBar.Value = 60;
                    lblStatus.Text = "Unpacking native resources.pri and DLLs...";
                    await Task.Delay(400);

                    // Extract payload
                    ZipFile.ExtractToDirectory(tempZip, installPath, overwriteFiles: true);
                    File.Delete(tempZip);
                }
            }

            progressBar.Value = 85;
            lblStatus.Text = "Registering system shortcuts...";
            await Task.Delay(300);

            string exePath = Path.Combine(installPath, "AxioVital.Desktop.exe");

            if (chkDesktopShortcut.Checked)
            {
                string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "AxioVital Desktop.lnk");
                CreateShortcut(exePath, desktopPath, "AxioVital Environment ™");
            }

            if (chkStartMenuShortcut.Checked)
            {
                string startMenuDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "AxioVital");
                Directory.CreateDirectory(startMenuDir);
                string startMenuPath = Path.Combine(startMenuDir, "AxioVital Desktop.lnk");
                CreateShortcut(exePath, startMenuPath, "AxioVital Environment ™");
            }

            progressBar.Value = 100;
            lblStatus.Text = "Installation complete.";
            await Task.Delay(400);

            ShowStep(3);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Installation Error: " + ex.Message, "AxioVital Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ShowStep(1);
        }
    }

    private static void CreateShortcut(string targetPath, string shortcutPath, string description)
    {
        try
        {
            string script = $"$WshShell = New-Object -ComObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('{shortcutPath}'); $Shortcut.TargetPath = '{targetPath}'; $Shortcut.Description = '{description}'; $Shortcut.Save()";
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
                CreateNoWindow = true,
                UseShellExecute = false
            });
            process?.WaitForExit();
        }
        catch { }
    }
}
