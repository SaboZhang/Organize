﻿using CommunityToolkit.WinUI;
using EasyTidy.Model;
using EasyTidy.Util;
using System.Runtime.CompilerServices;

namespace EasyTidy.ViewModels;

public partial class GeneralViewModel : ObservableRecipient
{
    #region 字段 & 属性

    /// <summary>
    ///     当前配置实例
    /// </summary>
    private static ConfigModel? CurConfig = Settings.GeneralConfig;

    [ObservableProperty]
    private bool pathTypeSelectedIndex = false;

    [ObservableProperty]
    private bool webDavIsShow = false;

    [ObservableProperty]
    public string floderPath;

    /// <summary>
    ///     文件冲突操作方式
    /// </summary>
    [ObservableProperty]
    private IList<FileOperationType> fileOperationTypes = Enum.GetValues(typeof(FileOperationType)).Cast<FileOperationType>().ToList();

    [ObservableProperty]
    private FileOperationType _operationType = CurConfig.FileOperationType;

    /// <summary>
    ///     备份类型
    /// </summary>
    [ObservableProperty]
    private IList<BackupType> backupTypes = Enum.GetValues(typeof(BackupType)).Cast<BackupType>().ToList();

    [ObservableProperty]
    private string _settingsBackupMessage;

    [ObservableProperty]
    private InfoBarSeverity _backupRestoreMessageSeverity;

    [ObservableProperty]
    private bool _settingsBackupRestoreMessageVisible;

    [ObservableProperty]
    private string _webDavUserName;

    [ObservableProperty]
    private string _webDavPassWord;

    [ObservableProperty]
    private string _webDavUrl;

    [ObservableProperty]
    private string _backupStatus;

    [ObservableProperty]
    private string _backupCreateTime;

    [ObservableProperty]
    private string _backupHostName;

    [ObservableProperty]
    private string _backupFileName;

    [ObservableProperty]
    private int _backupTypeIndex = -1;

    /// <summary>
    /// 是否处理子文件夹
    /// </summary>
    private bool? _subFolder;

    public bool SubFolder
    {
        get
        {
            return (bool)(_subFolder ?? CurConfig.SubFolder);
        }

        set
        {
            if (_subFolder != value)
            {
                _subFolder = value;
                CurConfig.SubFolder = value;
                NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    /// 是否处理使用中的文件
    /// </summary>
    private bool? _fileInUse;

    public bool FileInUse
    {
        get
        {
            return (bool)(_fileInUse ?? CurConfig.FileInUse);
        }

        set
        {
            if (_fileInUse != value)
            {
                _fileInUse = value;
                CurConfig.FileInUse = value;
                NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    ///     是否最小化到托盘
    /// </summary>
    private bool? _minimize;

    public bool Minimize
    {
        get
        {
            return (bool)(_minimize ?? CurConfig.Minimize);
        }

        set
        {
            if (_minimize != value)
            {
                _minimize = value;
                CurConfig.Minimize = value;
                NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    ///     是否处理无关文件
    /// </summary>
    private bool? _irrelevantFiles;

    public bool IrrelevantFiles
    {
        get
        {
            return (bool)(_irrelevantFiles ?? CurConfig.IrrelevantFiles);
        }

        set
        {
            if (_irrelevantFiles != value)
            {
                _irrelevantFiles = value;
                CurConfig.IrrelevantFiles = value;
                NotifyPropertyChanged();
            }
        }
    }

    private bool? _isStartupCheck;

    public bool IsStartupCheck
    {
        get
        {
            return (bool)(_isStartupCheck ?? CurConfig.IsStartupCheck);
        }

        set
        {
            if (_isStartupCheck != value)
            {
                _isStartupCheck = value;
                CurConfig.IsStartupCheck = value;
                NotifyPropertyChanged();
            }
        }
    }
    /// <summary>
    ///     是否开机启动
    /// </summary>
    private bool? _isStartup;

    public bool IsStartup
    {
        get
        {
            return (bool)(_isStartup ?? CurConfig.IsStartup);
        }

        set
        {
            if (_isStartup != value)
            {
                _isStartup = value;
                CurConfig.IsStartup = value;
                NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    ///     是否处理空文件或者空文件夹
    /// </summary>
    private bool? _emptyFiles;

    public bool EmptyFiles
    {
        get
        {
            return (bool)(_emptyFiles ?? CurConfig.EmptyFiles);
        }

        set
        {
            if (_emptyFiles != value)
            {
                _emptyFiles = value;
                CurConfig.EmptyFiles = value;
                NotifyPropertyChanged();
            }
        }
    }

    /// <summary>
    ///     是否处理隐藏文件
    /// </summary>
    private bool? _isHiddenFiles;

    public bool HiddenFiles
    {
        get
        {
            return (bool)(_isHiddenFiles ?? CurConfig.HiddenFiles);
        }

        set
        {
            if (_isHiddenFiles != value)
            {
                _isHiddenFiles = value;
                CurConfig.HiddenFiles = value;
                NotifyPropertyChanged();
            }
        }
    }

    private bool? _autoBackup;

    public bool AutoBackup
    {
        get
        {
            return _autoBackup ?? Settings.BackupConfig.AutoBackup;
        }

        set
        {
            if (_autoBackup != value)
            {
                _autoBackup = value;
                Settings.BackupConfig.AutoBackup = value;
                NotifyPropertyChanged();
            }
        }
    }

    #endregion

    [RelayCommand]
    private void OnPageLoaded()
    {
        BackupStatus = Settings.BackupConfig.BackupVersion ?? string.Empty;
        BackupFileName = Settings.BackupConfig.BackupFileName ?? string.Empty;
        BackupHostName = Settings.BackupConfig.HostName ?? string.Empty;
        BackupCreateTime = Settings.BackupConfig.CreateTime ?? string.Empty;
        WebDavUrl = Settings.WebDavUrl ?? string.Empty;
        WebDavUserName = Settings.WebDavUser ?? string.Empty;
        WebDavPassWord = Settings.WebDavPassword ?? string.Empty;
        WebDavIsShow = Settings.BackupType == BackupType.WebDav;
        BackupTypeIndex = WebDavIsShow? 1 : PathTypeSelectedIndex? 0 : -1;
    }

    [RelayCommand]
    private void OnFileOperationTypeChanged(object sender)
    {
        UpdateCurConfig(this);
    }

    [RelayCommand]
    private void OnSelectPathType(object sender)
    {
        var backupType = sender as ComboBox;
        switch (backupType.SelectedItem)
        {
            case BackupType.Local:
                PathTypeSelectedIndex = true;
                WebDavIsShow = false;
                Settings.BackupType = BackupType.Local;
                break;
            case BackupType.WebDav:
                PathTypeSelectedIndex = false;
                WebDavIsShow = true;
                Settings.BackupType = BackupType.WebDav;
                break;
            default:
                PathTypeSelectedIndex = false;
                WebDavIsShow = false;
                break;
        }
    }

    [RelayCommand]
    private async Task OnSelectPath()
    {
        try
        {
            var folder = await FileAndFolderPickerHelper.PickSingleFolderAsync(App.MainWindow);
            FloderPath = folder?.Path ?? (WebDavIsShow ? "WebDAV" : "");

        }
        catch (Exception ex)
        {
            FloderPath = "";
            Logger.Error($"ServerViewModel: OnSelectMediaPath 异常信息 {ex}");
        }
    }

    [RelayCommand]
    private async Task BackupConfigsClickAsync()
    {
        switch (Settings.BackupType)
        {
            case BackupType.Local:
                // 本地备份 
                await LocalBackupAsync();
                break;
            case BackupType.WebDav:
                // WebDav备份
                await WebDavBackupAsync();
                break;
            default:
                SettingsBackupRestoreMessageVisible = true;
                BackupRestoreMessageSeverity = InfoBarSeverity.Warning;
                SettingsBackupMessage = "BackupSelectionTips".GetLocalized();
                await DelayCloseMessageVisible();
                break;
        }
        
    }

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = null, bool reDoBackupDryRun = true)
    {

        // Notify UI of property change
        OnPropertyChanged(propertyName);

        Logger.Info($"GeneralViewModel: NotifyPropertyChanged {propertyName}");

        UpdateCurConfig(this);

    }

    private async Task LocalBackupAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(FloderPath))
            {
                SettingsBackupRestoreMessageVisible = true;
                BackupRestoreMessageSeverity = InfoBarSeverity.Warning;
                SettingsBackupMessage = "BackupPathText".GetLocalized();
                await DelayCloseMessageVisible();
                return;
            }
            var zipFilePath = Path.Combine(FloderPath, $"EasyTidy_backup_{DateTime.Now:yyyyMMddHHmmss}.zip");
            ZipUtil.CompressFile(Constants.CnfPath, zipFilePath);
            SettingsBackupRestoreMessageVisible = true;
            ShowBackInfo(zipFilePath);
            BackupRestoreMessageSeverity = InfoBarSeverity.Success;
            SettingsBackupMessage = "BackupSuccessTips".GetLocalized();
        }
        catch (Exception ex) 
        { 
            Logger.Error($"备份异常：{ex.Message}");
            SettingsBackupRestoreMessageVisible = true;
            BackupRestoreMessageSeverity = InfoBarSeverity.Error;
            SettingsBackupMessage = "BackupFailedTips".GetLocalized();
        }
        finally
        {
            await DelayCloseMessageVisible();
        }
    }

    private void ShowBackInfo(string file)
    {

        BackupStatus = AssemblyInfoHelper.GetAssemblyVersion();
        BackupCreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        BackupFileName = Path.GetFileName(file);
        BackupHostName = WebDavIsShow? WebDavUrl + "/EasyTidy" : Environment.MachineName;

        Settings.BackupConfig.BackupVersion = BackupStatus;
        Settings.BackupConfig.CreateTime = BackupCreateTime;
        Settings.BackupConfig.BackupFileName = BackupFileName;
        Settings.BackupConfig.HostName = BackupHostName;
        Settings.Save();
    }

    private async Task WebDavBackupAsync()
    {
        try
        {
            if (!string.IsNullOrEmpty(Settings.WebDavPassword))
            {
                WebDavPassWord = DESUtil.DesDecrypt(Settings.WebDavPassword);
            }
            WebDavClient webDavClient = new(Settings.WebDavUrl ??= WebDavUrl, Settings.WebDavUser ??= WebDavUserName, WebDavPassWord);
            Settings.WebDavPassword ??= DESUtil.DesEncrypt(WebDavPassWord);
            var zipFilePath = Path.Combine(Constants.CnfPath, $"EasyTidy_backup_{DateTime.Now:yyyyMMddHHmmss}.zip");
            ZipUtil.CompressFile(Constants.CnfPath, zipFilePath);
            var backup = await webDavClient.UploadFileAsync(WebDavUrl + "/EasyTidy", zipFilePath);
            SettingsBackupRestoreMessageVisible = backup;
            BackupRestoreMessageSeverity = InfoBarSeverity.Success;
            SettingsBackupMessage = "BackupSuccessTips".GetLocalized();
            ShowBackInfo(zipFilePath);
        }
        catch (Exception ex)
        {
            Logger.Error($"{ex.Message}");
            BackupRestoreMessageSeverity = InfoBarSeverity.Error;
            SettingsBackupMessage = "BackupFailedTips".GetLocalized();
        }
        finally
        {
            await DelayCloseMessageVisible();
        }
    }

    private async Task DelayCloseMessageVisible()
    {
        await Task.Delay(10000);
        SettingsBackupRestoreMessageVisible = false;
    }

}
