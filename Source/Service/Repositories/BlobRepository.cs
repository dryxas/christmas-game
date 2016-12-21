namespace Christmas2016.Service.Repositories
{
    using AForge;
    using AForge.Imaging;
    using AForge.Imaging.Filters;
    using AForge.Video;
    using AForge.Video.DirectShow;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class BlobRepository : IBlobRepository
    {
        private readonly Thread trackingThread;
        private readonly VideoCaptureDevice videoCaptureDevice;
        private Bitmap bitmap;
        private List<Blob> blobs = new List<Blob>();

        public BlobRepository()
        {
            var videoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            this.videoCaptureDevice = new VideoCaptureDevice(videoCaptureDevices[2].MonikerString);
            var videoCapabilities = this.videoCaptureDevice.VideoCapabilities;
            int len = this.videoCaptureDevice.VideoCapabilities.Length;
            for (int i = 0; i < len; i++)
            {
                System.Console.WriteLine("i={0} FrameSize={1}", i.ToString(), this.videoCaptureDevice.VideoCapabilities[i].FrameSize.ToString());
            }
            this.videoCaptureDevice.VideoResolution = videoCapabilities[17];
            this.videoCaptureDevice.NewFrame += this.HandleNewFrame;
            this.videoCaptureDevice.Start();

            this.trackingThread = new Thread(this.DetectPointer);
            this.trackingThread.Start();
        }

        public async Task<List<Blob>> GetAsync() => await Task.FromResult(this.blobs);

        private static Bitmap ApplyEuclideanColorFilter(Bitmap bitmap)
        {
          // create filter
          // ColorFiltering filter = new ColorFiltering( );
          // // set color ranges to keep
          // filter.Red   = new IntRange( 30, 100 );
          // filter.Green = new IntRange( 60, 180 );
          // filter.Blue  = new IntRange( 120, 255 );
          // // apply the filter
          //
          // var filteredBitmap = filter.Apply( bitmap );
            var filter = new EuclideanColorFiltering
            {
                CenterColor = new RGB(Color.FromArgb(10, 79, 170)),
                Radius = 70
            };

            var filteredBitmap = filter.Apply(bitmap);

            return filteredBitmap;
        }

        private static Bitmap ApplyHslFilter(Bitmap bitmap)
        {
            var filter = new HSLFiltering
            {
                Hue = new IntRange(335, 0),
                Saturation = new Range(0.6f, 1),
                Luminance = new Range(0.1f, 1)
            };

            var filteredBitmap = filter.Apply(bitmap);

            return filteredBitmap;
        }

        private static Bitmap ApplyYCbCrFilter(Bitmap bitmap)
        {
            var filter = new YCbCrFiltering
            {
                Cb = new Range(-0.2f, 0),
                Cr = new Range(0.26f, 0.5f)
            };

            var filteredBitmap = filter.Apply(bitmap);

            return filteredBitmap;
        }

        private void DetectPointer()
        {
            while (true)
            {
                Thread.Sleep(10);

                if (this.bitmap == null)
                {
                    continue;
                }

                var blobCounter = new BlobCounter
                {
                    MinWidth = 25,
                    MinHeight = 25,
                    FilterBlobs = true,
                    ObjectsOrder = ObjectsOrder.Area
                };

                // var bitmap = ApplyEuclideanColorFilter(this.bitmap);
                // bitmap.Save(@"C:\Projects\debug.png");

                blobCounter.ProcessImage(ApplyEuclideanColorFilter(this.bitmap));
                this.blobs = new List<Blob>(blobCounter.GetObjectsInformation());
            }
        }

        private void HandleNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                this.bitmap = (Bitmap)eventArgs.Frame.Clone();
            }
            finally
            {
                // shit happens
            }
        }
    }
}
