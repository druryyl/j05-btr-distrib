using j07_btrade_sync.Repository;
using j07_btrade_sync.Service;
using j07_btrade_sync.Shared;
using Nuna.Lib.ValidationHelper;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace j07_btrade_sync
{
    public partial class SyncForm : Form
    {
        private readonly BrgSyncService _brgSyncService;
        private readonly CustomerUploadService _customerSyncService;
        private readonly SalesPersonSyncService _salesPersonSyncService;
        private readonly KategoriSyncService _kategoriSyncService;
        private readonly WilayahSyncService _wilayahSyncService;
        private readonly OrderIncrementalDownloadService _orderDownloadService;
        private readonly CustomerDownloadUpdatedService _customerDownloadUpdatedService;
        private readonly CustomerClearUpdateFlagService _customerClearUpdateFlagService;
        private readonly CheckInIncrementalDownloadService _checkInDownloadService;

        private readonly BrgDal _brgDal;
        private readonly CustomerDal _customerDal;
        private readonly SalesPersonDal _salesPersonDal;
        private readonly KategoriDal _kategoriDal;
        private readonly WilayahDal _wilayahDal;
        private readonly OrderDal _orderDal;
        private readonly OrderItemDal _orderItemDal;
        private readonly CheckInDal _checkInDal;

        private Timer timerClock = new Timer();
        private System.Timers.Timer processingTimer;
        private int processingIntervalMinutes = 5;
        private const int RANGE_PERIODE = -3;
        private readonly RegistryHelper _registryHelper;


        public SyncForm()
        {
            InitializeComponent();

            _brgSyncService = new BrgSyncService();
            _customerSyncService = new CustomerUploadService();
            _salesPersonSyncService = new SalesPersonSyncService();
            _kategoriSyncService = new KategoriSyncService();
            _wilayahSyncService = new WilayahSyncService();
            _orderDownloadService = new OrderIncrementalDownloadService();
            _customerDownloadUpdatedService = new CustomerDownloadUpdatedService();
            _customerClearUpdateFlagService = new CustomerClearUpdateFlagService();
            _checkInDownloadService = new CheckInIncrementalDownloadService();

            _brgDal = new BrgDal();
            _customerDal = new CustomerDal();
            _salesPersonDal = new SalesPersonDal();
            _kategoriDal = new KategoriDal();
            _wilayahDal = new WilayahDal();
            _orderDal = new OrderDal();
            _orderItemDal = new OrderItemDal();
            _checkInDal = new CheckInDal();
            _registryHelper = new RegistryHelper();

            InitializeTimer();
            InitializeClock();
            RegisterEventHandler();
            LogMessage("BTrade Sync started.");
            ProcessOrder(RANGE_PERIODE);
            ProcessCheckIn(RANGE_PERIODE);
            ShowServerTarget();

            var nextAuto = DateTime.Now.AddMinutes(processingIntervalMinutes);
            LogMessage($"Next auto download start at {nextAuto:HH:mm:ss}");

        }

        private void ShowServerTarget()
        {
            var server = _registryHelper.ReadString("ServerTargetID");
            this.Text = $"BTrade Sync - Server ID: {server}";
        }

        private void RegisterEventHandler()
        {
            SyncBrgButton.Click += SyncBrgButton_Click;
            SyncCustomerButton.Click += SyncCustomerButton_Click;
            SyncSalesPersonBotton.Click += SyncSalesPersonButton_Click;

            // Create context menu
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Quick (H-3)", null, (s, e) => QuickDownloadOrderButton_Click(s,e));
            menu.Items.Add("Extended  (H-6)", null, (s, e) => ExtenderdDownloadOrderButton_Click(s, e));
            menu.Items.Add("-", null);
            menu.Items.Add("Set Server ID", null, (s, e) => SetServerId_Click(s, e));


            // Attach menu to button
            IncrementalDownloadOrderButton.ContextMenuStrip = menu;

            // Optional: Show menu on click
            IncrementalDownloadOrderButton.Click += (s, e) => {
                menu.Show(IncrementalDownloadOrderButton, new Point(0, IncrementalDownloadOrderButton.Height));
            };
        }

        private void SetServerId_Click(object s, EventArgs e)
        {
            var dialogResult = MessageBox.Show("Set Server ID akan mempengaruhi transfer data antar cabang. Lanjutkan?", "X", MessageBoxButtons.YesNoCancel);
            if ( dialogResult != DialogResult.Yes)
                return;
            var formKonfig = new KonfigurasiForm();
            formKonfig.ShowDialog();
        }

        public void LogMessage(string message, Color? color = null)
        {
            if (LogTextBox.InvokeRequired)
            {
                LogTextBox.Invoke(new Action(() => LogMessage(message, color)));
                return;
            }

            color = color ?? Color.Black;

            LogTextBox.SelectionStart = LogTextBox.TextLength;
            LogTextBox.SelectionLength = 0;
            LogTextBox.SelectionColor = color.Value;
            LogTextBox.AppendText($"{DateTime.Now:HH:mm:ss} - {message}{Environment.NewLine}");
            LogTextBox.SelectionColor = LogTextBox.ForeColor;
            LogTextBox.ScrollToCaret();
        }

        private async void ProcessOrder(int periodeLength)
        {
            if (periodeLength > 0)
                periodeLength = periodeLength * -1;
            try
            {
                LogMessage("Starting processing cycle...");
                var today = DateTime.Now.Date;
                var startDate = today.AddDays(periodeLength);
                var periode = new Periode(startDate, today);
                var result = await _orderDownloadService.Execute(periode);
                
                if (result.Item1)
                {
                    var listOrder = result.Item3;
                    if (listOrder != null && listOrder.Any())
                    {
                        foreach (var order in listOrder)
                        {
                            var orderDb = _orderDal.GetData(order);
                            if (orderDb != null)
                                continue;

                            _orderDal.Insert(order);
                            _orderItemDal.Delete(order);
                            _orderItemDal.Insert(order.ListItems);
                            LogMessage($"Download order {order.SalesName} - {order.CustomerName} ...", Color.Blue);
                        }
                        LogMessage($"Download done");
                    }
                    else
                    {
                        LogMessage("No orders found");
                    }
                }
                else
                {
                    LogMessage($"Download failed: {result.Item2}", Color.Red);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"ERROR: {ex.Message}", Color.Red);
            }
        }

        private async void ProcessCheckIn(int periodeLength)
        {
            if (periodeLength > 0)
                periodeLength = periodeLength * -1;
            try
            {
                LogMessage("Starting check-in processing cycle...");
                var today = DateTime.Now.Date;
                var startDate = today.AddDays(periodeLength);
                var periode = new Periode(startDate, today);
                var result = await _checkInDownloadService.Execute(periode);

                if (result.Item1)
                {
                    var listCheckIn = result.Item3;
                    if (listCheckIn != null && listCheckIn.Any())
                    {
                        foreach (var checkIn in listCheckIn)
                        {
                            var checkInDb = _checkInDal.GetData(checkIn);
                            if (checkInDb != null)
                                continue;

                            _checkInDal.Insert(checkIn);
                            LogMessage($"Download check-in {checkIn.UserEmail} - {checkIn.CustomerName} ...", Color.Blue);
                        }
                        LogMessage($"Check-in download done");
                    }
                    else
                    {
                        LogMessage("No check-ins found");
                    }
                }
                else
                {
                    LogMessage($"Check-in download failed: {result.Item2}", Color.Red);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"CHECK-IN ERROR: {ex.Message}", Color.Red);
            }
        }

        private void InitializeTimer()
        {
            processingTimer = new System.Timers.Timer(processingIntervalMinutes * 60 * 1000);
            processingTimer.Elapsed += ProcessingTimer_Elapsed;
            processingTimer.AutoReset = false; // We'll manually restart after processing
            processingTimer.Start();
        }
        private void InitializeClock()
        {
            var timeLabel = new ToolStripStatusLabel();
            timeLabel.BackColor = Color.WhiteSmoke;
            timeLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            timeLabel.ForeColor = Color.Gray;
            statusStrip1.Items.Add(timeLabel);

            timerClock.Interval = 1000;
            timerClock.Tick += (s, e) => {
                timeLabel.Text = $"Running Timer {DateTime.Now.ToString("HH:mm:ss")}";
            };
            timerClock.Start();
        }
        private async void ProcessingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.BeginInvoke((Action)(() =>
            {
                //LogMessage($"Timer triggered - Starting processing at {DateTime.Now}");
            }));

            try
            {
                await Task.Run(() => ProcessOrder(RANGE_PERIODE)); // Run processing on background thread
                await Task.Run(() => ProcessCustomer());
                await Task.Run(() => ProcessCheckIn(RANGE_PERIODE));
            }
            catch (Exception ex)
            {
                this.BeginInvoke((Action)(() =>
                {
                    LogMessage($"Processing error: {ex.Message}", Color.Red);
                }));
            }
            finally
            {
                processingTimer.Start(); // Restart the timer
                var nextAuto = DateTime.Now.AddMinutes(processingIntervalMinutes);
                LogMessage($"Next auto download start at {nextAuto:HH:mm:ss}");
            }
        }

        private void QuickDownloadOrderButton_Click(object sender, EventArgs e)
        {
            LogMessage($"Quick Download triggered", Color.Green);
            ProcessOrder(-3);
            ProcessCustomer();
            ProcessCheckIn(-3);
        }
        private void ExtenderdDownloadOrderButton_Click(object sender, EventArgs e)
        {
            LogMessage($"Extended Download triggered", Color.Green);
            ProcessOrder(-6);
            ProcessCustomer();
            ProcessCheckIn(-6);
        }


        private async void ProcessCustomer()
        {
            try
            {
                LogMessage("Start processing customer location...");
                var result = await _customerDownloadUpdatedService.Execute();

                if (result.Item1)
                {
                    var listCustomer = result.Item3;
                    if (listCustomer != null && listCustomer.Any())
                    {
                        foreach (var cust in listCustomer)
                        {
                            var custDb = _customerDal.GetData(cust.CustomerId);
                            if (custDb is null)
                                continue;
                            _customerDal.UpdateLocation(cust);
                            LogMessage($"Download location {cust.CustomerName} [{cust.Latitude:N5}, {cust.Longitude:N5}] ...", Color.Blue);
                        }
                        LogMessage($"Download done");
                        await _customerClearUpdateFlagService.Execute();
                    }
                    else
                    {
                        LogMessage("No new location found");
                    }
                }
                else
                {
                    LogMessage($"Download failed: {result.Item2}", Color.Red);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"ERROR: {ex.Message}", Color.Red);
            }
        }
        private void SyncCustomerButton_Click(object sender, EventArgs e)
        {
            ProcessCustomer();
            LogMessage("Upload Customer started...", Color.Green);
            var listCustomer = _customerDal.ListData().ToList();
            var result = _customerSyncService.UploadCustomer(listCustomer);
            result.ContinueWith(task =>
            {
                if (task.Result.Item1)
                {
                    LogMessage("Done");
                }
                else
                {
                    LogMessage($"Upload Customer failed: {task.Result.Item2}", Color.Red);
                }
            });
        }

        private void SyncBrgButton_Click(object sender, EventArgs e)
        {
            LogMessage("Sync Barang started...", Color.Green);
            var listBrg = _brgDal.ListData().ToList();
            var result = _brgSyncService.SyncBrg(listBrg);
            result.ContinueWith(task =>
            {
                if (task.Result.Item1)
                {
                    LogMessage("Done");
                }
                else
                {
                    LogMessage($"Sync failed: {task.Result.Item2}", Color.Red);
                    return;
                }
            });

            var listKategori = _kategoriDal.ListData().ToList();
            var kategoriResult = _kategoriSyncService.SyncKategori(listKategori);
            kategoriResult.ContinueWith(task =>
            {
                if (task.Result.Item1)
                {
                    //MessageBox.Show("Brg-Kategori sync successful!");
                }
                else
                {
                    //MessageBox.Show($"Kategori sync failed: {task.Result.Item2}");
                }
            });

        }
        private void SyncSalesPersonButton_Click(object sender, EventArgs e)
        {
            LogMessage("Sync Sales started...", Color.Green);
            var listSalesPerson = _salesPersonDal.ListData().ToList();
            var result = _salesPersonSyncService.SyncSalesPerson(listSalesPerson);
            result.ContinueWith(task =>
            {
                if (task.Result.Item1)
                {
                    LogMessage("Done");
                }
                else
                {
                    LogMessage($"Sync failed: {task.Result.Item2}", Color.Red);
                }
            });

            var listWilayah = _wilayahDal.ListData().ToList();
            var wilayahResult = _wilayahSyncService.SyncWilayah(listWilayah);
            wilayahResult.ContinueWith(task =>
            {
                if (task.Result.Item1)
                {
                    //MessageBox.Show("SalesPerson-Wilayah sync successful!");
                }
                else
                {
                    //MessageBox.Show($"Wilayah sync failed: {task.Result.Item2}");
                }
            });
        }

    }
}
