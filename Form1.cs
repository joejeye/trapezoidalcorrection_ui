using ImageDistorsion.PixelLayer;
using ImageDistorsion.FormattingLayer;
using ImageDistorsion.NumericLayer;
using ImageDistorsion;
using MathNet.Numerics.LinearAlgebra;
using ImageDistorsion.NumericLayer.NumericVisualization;
using System.Drawing;

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
                DisplayedImg = new(ofd.FileName);
                pictureBox1.Image = DisplayedImg;
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

        /// <summary>
        /// The image displayed in the PictureBox.
        /// </summary>
        private Bitmap? DisplayedImg;

        /// <summary>
        /// For storing the corrected image.
        /// </summary>
        private PixelArrayToBitmap? PA2Bmp;

        /* Correct the distortion in the image and display the corrected image. */
        private void ProcessImg(int widthInPixs, int heightInPixs)
        {
            if (ImagePath == null || MarkerCnt < 4)
            {
                MessageBox.Show("Please load an image and place 4 markers on it.");
                return;
            }

            /*
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
            */

            /* Make a copy of the color array of the image. */
            ArgumentNullException.ThrowIfNull(DisplayedImg);
            ColorArray2D colorArr = new(DisplayedImg);

            /* Map the four vertices of the quadrilateral from the UI row-col indexes to image row-col indexes*/
            UIPixToImgPix ui2img = new(pictureBox1.Width, pictureBox1.Height, colorArr.NHorizontalPix, colorArr.NVerticalPix);
            List<RowColForHash> VertsOnImg = ui2img.ConvertAll(RecordedMarkers);

            /* Get the bounding box of the quadrilateral. */
            BoundingFrame boundingFrame = new(VertsOnImg.ToArray());
            boundingFrame.FinishReading();

            /* Crop the image to the bounding box. */
            colorArr.Crop([boundingFrame.TopLeftRowCol.RowIdx, boundingFrame.TopLeftRowCol.ColIdx], [boundingFrame.BottomRightRowCol.RowIdx, boundingFrame.BottomRightRowCol.ColIdx], ref VertsOnImg);

            /* Create the distorted visualization mapping. */
            // Get the nearest-lattice-point mapping
            RowCol_Coord_Mapping NrstLttcPntMapping = new(colorArr.NHorizontalPix, colorArr.NVerticalPix);
            // The mapping from the distorted image pixel indexes to the color
            DistortedVisualizationMapping DstVisMapping = new((int rowIdx, int colIdx) => colorArr[rowIdx, colIdx], NrstLttcPntMapping);

            /* Create the numeric layer pullback correction mapping. */
            // Create the numeric layer corrected domain
            RectPolygon correctedDomain = new(new double[] { 0, 0 }, new double[] { 1, 1 });
            // Create the numeric layer quadrilateral
            Coord2ForHash<double>[] quadVertices = (from v in VertsOnImg 
                                                    select DstVisMapping.Coord2RowColMapping.RowColToCoordMapping(v.RowIdx, v.ColIdx)).ToArray(); // Get the four vertices of the quadrilateral
            ConvexPolygon quadrilateral = new((from qv in quadVertices
                                               select new double[] { qv.x, qv.y }).ToArray());
            // The pullback correction mapping
            ImageDistorsion.NumericLayer.PullbackCorrection NuPullbackCorrection = new(quadrilateral, correctedDomain, (double x, double y) => DstVisMapping.Map(x, y));

            /* Create the pixel layer pullback correction. */
            // Get the mapping from the row and column indexes to the Euclidean plane coordinates for the corrected image
            RowCol_Coord_Mapping RowCol2CoordForCorrected = new(widthInPixs, heightInPixs, xspan: 1, yspan: 1);
            // The pixel layer pullback correction
            ImageDistorsion.PixelLayer.PullbackCorrection PixelLayerPullbackCorrection = new(RowCol2CoordForCorrected, NuPullbackCorrection);

            /* Generate the pixel array for the corrected image. */
            Color[,] PixArrForCorrected = new Color[widthInPixs, heightInPixs]; // Index pair is in the form of [column, row]
            for (int i = 0; i < heightInPixs; i++) // Row index
            {
                for (int j = 0; j < widthInPixs; j++) // Column index
                {
                    PixArrForCorrected[j, i] = PixelLayerPullbackCorrection.CorrectedRowColToColorMapping(i, j);
                }
            }

            /* Prepare the corrected image for display. */
            PA2Bmp = new(PixArrForCorrected);

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
                    ArgumentNullException.ThrowIfNull(PA2Bmp);
                    PA2Bmp.ImgBmp.Save(sfd.FileName);
                    MessageBox.Show("Image saved successfully");
                }
            }
        }
    }
}
