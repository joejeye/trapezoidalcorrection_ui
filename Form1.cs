using ImageDistorsion.PixelLayer;
using ImageDistorsion.FormattingLayer;
using ImageDistorsion.NumericLayer;
using ImageDistorsion;
using MathNet.Numerics.LinearAlgebra;
using ImageDistorsion.NumericLayer.NumericVisualization;

namespace ImageDistorsionUI
{
    using VecDbl = Vector<double>;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string? ImagePath = null;

        private void loadBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new();
            ofd.Filter = "Bitmap Image (*.bmp)|*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(ofd.FileName);
                ImagePath = ofd.FileName;
            }
        }

        /// <summary>
        /// The number of markers the user has placed on the image.
        /// </summary>
        private int MarkerCnt = 0;

        private List<Point> RecordedMarkers = new();

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            /* The user can place up to 4 markers on the image. */
            if (MarkerCnt >= 4)
            {
                return;
            }
            MarkerCnt++;

            /* Record the position of the marker. */
            int x = e.X; // Distance from the left boundary of the PictureBox
            int y = e.Y; // Distance from the top boundary of the PictureBox
            RecordedMarkers.Add(new Point(x, y));

            /* Draw the marker on the image. */
            Graphics g = pictureBox1.CreateGraphics();
            g.FillEllipse(Brushes.Red, x - 5, y - 5, 10, 10);

            /* Draw a line between each pair of markers. */
            if (MarkerCnt > 1)
            {
                /* Draw a line between the last two markers. */
                g.DrawLine(Pens.Red, RecordedMarkers[MarkerCnt - 2], RecordedMarkers[MarkerCnt - 1]);

                /* If the user has placed 4 markers, draw a line between the first and last markers. */
                if (MarkerCnt == 4)
                {
                    g.DrawLine(Pens.Red, RecordedMarkers[3], RecordedMarkers[0]);
                }
            }
        }

        private bool isDragging = false;
        private Point mousePosition;
        private int draggedMarkerIndex = -1;

        /// <summary>
        /// When the user presses and holds the left mouse button, check if the mouse is over a marker.
        /// If it is, set isDragging to true and record the position of the mouse.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < RecordedMarkers.Count; i++)
            {
                if (Math.Abs(e.X - RecordedMarkers[i].X) <= 5 && Math.Abs(e.Y - RecordedMarkers[i].Y) <= 5)
                {
                    isDragging = true;
                    mousePosition = e.Location;
                    draggedMarkerIndex = i;
                    break;
                }
            }
        }

        /* When the user is dragging a marker, update the position of the marker and redraw the quadrilateral. */
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            /* If the user is dragging a marker, update the position of the marker and redraw the quadrilateral. */
            if (isDragging && draggedMarkerIndex != -1)
            {
                /* Update the position of the marker. */
                RecordedMarkers[draggedMarkerIndex] = new Point(RecordedMarkers[draggedMarkerIndex].X + e.X - mousePosition.X, RecordedMarkers[draggedMarkerIndex].Y + e.Y - mousePosition.Y);
                mousePosition = e.Location;

                /* Redraw the quadrilateral. */
                pictureBox1.Refresh();
                Graphics g = pictureBox1.CreateGraphics();
                for (int i = 0; i < RecordedMarkers.Count; i++)
                {
                    g.FillEllipse(Brushes.Red, RecordedMarkers[i].X - 5, RecordedMarkers[i].Y - 5, 10, 10);
                    if (i > 0)
                    {
                        g.DrawLine(Pens.Red, RecordedMarkers[i - 1], RecordedMarkers[i]);
                    }
                }
                if (RecordedMarkers.Count == 4)
                {
                    g.DrawLine(Pens.Red, RecordedMarkers[3], RecordedMarkers[0]);
                }
            }
        }

        /* When the user releases the left mouse button, set isDragging to false. */
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            draggedMarkerIndex = -1;
        }

        /* Clear the markers and the quadrilateral. */
        private void ResetMarkers()
        {
            MarkerCnt = 0;
            RecordedMarkers.Clear();
            pictureBox1.Refresh();
        }

        /* When the user clicks the "Reset markers" button, reset the markers. */
        private void rstBtn_MouseClick(object sender, MouseEventArgs e)
        {
            ResetMarkers();
        }

        private void LogMarkerLocatinos()
        {
            if (RecordedMarkers.Count == 0)
            {
                label1.Text = "No markers have been placed.";
                return;
            }
            string LastMarker = RecordedMarkers[RecordedMarkers.Count - 1].ToString();
            label1.Text = LastMarker;
        }

        /* When the user clicks the "Log locations" button, log the positions of the markers. */
        private void logBtn_Click(object sender, EventArgs e)
        {
            LogMarkerLocatinos();
            LogPictureBoxSize();
        }

        private void LogPictureBoxSize()
        {
            string Size = pictureBox1.Size.ToString();
            label2.Text = Size;
        }

        private PixelArrayToBitmap PA2Bmp;

        /* Correct the distortion in the image and display the corrected image. */
        private void ProcessImg(int widthInPixs, int heightInPixs)
        {
            if (ImagePath == null || MarkerCnt < 4)
            {
                MessageBox.Show("Please load an image and place 4 markers on it.");
                return;
            }

            ColorArray2D img = new(ImagePath);
            UIPixToImgPix ui2img = new(pictureBox1.Width, pictureBox1.Height, img.NHorizontalPix, img.NVerticalPix);
            List<RowColForHash> imgMarkers = ui2img.ConvertAll(RecordedMarkers);
            MarkersInRegion mir = new(
                img,
                (from imkr in imgMarkers select new int[] { imkr.RowIdx, imkr.ColIdx }).ToArray()
            );
            InverseDistort invDistort = new(mir.SelectedRegion, pictureBox1.Width, pictureBox1.Height); // WIP: The last two arguments are currently unused
            MarkersToPixels mtp = new(widthInPixs, heightInPixs); // WIP: The size of the corrected image is currently hardcoded. It should have been calculated from the quadrilateral based on the 3D orientation of the paper relative to the camera.
            foreach (var nmk in mir)
            {
                VecDbl correctedPoint = invDistort.Mapping(VecDbl.Build.DenseOfArray([nmk.x, nmk.y]));
                mtp.LoadMarker(new NuMarker<Color>(correctedPoint[0], correctedPoint[1], nmk.color));
            }
            mtp.FinishLoading();
            PA2Bmp = new(mtp.PixArray2D);

            /* Display the corrected image. */
            pictureBox1.Image = PA2Bmp.ImgBmp;
        }

        private void correctBtn_Click(object sender, EventArgs e)
        {
            if (MarkerCnt < 4)
            {
                return;
            }

            /* The user can specify the width and height of the corrected image. */
            string widthTxt = txtBoxWidthInput.Text;
            widthTxt = widthTxt.Trim();
            int widthInPixs = 480;
            if (widthTxt.All(char.IsDigit))
            {
                widthInPixs = int.Parse(widthTxt);
            }
            string heightTxt = txtBoxHeightInput.Text;
            heightTxt = heightTxt.Trim();
            int heightInPixs = 360;
            if (heightTxt.All(char.IsDigit))
            {
                heightInPixs = int.Parse(heightTxt);
            }

            ProcessImg(widthInPixs, heightInPixs);
            ResetMarkers();
        }

        /* Save the corrected image. */
        private void saveBtn_Click(object sender, EventArgs e)
        {
            // Pops up a save file dialog
            using (SaveFileDialog sfd = new())
            {
                sfd.Filter = "Bitmap Image (*.bmp)|*.bmp";
                sfd.RestoreDirectory = true;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    PA2Bmp.ImgBmp.Save(sfd.FileName);
                    MessageBox.Show("Image saved successfully");
                }
            }
        }
    }
}