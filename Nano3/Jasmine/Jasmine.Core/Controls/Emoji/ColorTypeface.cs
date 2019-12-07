using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Typography.OpenFont;
using Typography.TextLayout;

namespace Jasmine.Core.Controls.Emoji
{
    internal class ColorTypeface
    {
        public ColorTypeface(string name)
        {
            m_gtf = GetGlyphTypeface(first_candidate: name);
            if (m_gtf == null)
                return;

            // Read the actual font data using Typography.OpenFont
            using (var s = m_gtf.GetFontStream())
            {
                var r = new Typography.OpenFont.OpenFontReader();
                m_openfont = r.Read(s, 0, Typography.OpenFont.ReadFlags.Full);
            }

            // Create a layout for glyphs
            m_layout = new GlyphLayout();
            m_layout.ScriptLang = ScriptLangs.Default;
            m_layout.Typeface = m_openfont;
            m_layout.PositionTechnique = PositionTechnique.OpenFont;
        }

        private GlyphTypeface GetGlyphTypeface(string first_candidate)
        {
            IList<string> all_candidates = new List<string>();

            if (first_candidate != null)
                all_candidates.Add(first_candidate);

            // Some good Emoji font candidates
            all_candidates.Add("Segoe UI Emoji");
            all_candidates.Add(@"c:\Windows\Fonts\seguiemj.ttf");

            // Maybe try the Firefox EmojiOne font?
            var firefox_key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\firefox.exe";
            var firefox_path = Microsoft.Win32.Registry.GetValue(firefox_key, "Path", null);
            if (firefox_path is string)
                all_candidates.Add((firefox_path as string) + @"\fonts\EmojiOneMozilla.ttf");

            // Last resort fallbacks
            all_candidates.Add("Segoe UI Symbol"); // for older versions of Windows
            all_candidates.Add("Arial"); // available since Windows 3.1!

            foreach (var name in all_candidates)
            {
                var typeface = new System.Windows.Media.Typeface(name);
                if (typeface.TryGetGlyphTypeface(out var gtf))
                    return gtf;

                try
                {
                    return new GlyphTypeface(new Uri(name));
                }
                catch { }
            }

            return null;
        }

        /// <summary>
        /// Return whether the font can render the given string entirely
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool CanRender(string s)
        {
            m_layout.Layout(s.ToCharArray(), 0, s.Length);
            foreach (var g in m_layout.GetUnscaledGlyphPlanIter())
                if (g.glyphIndex == 0)
                    return false;
            return true;
        }

        public IUnscaledGlyphPlanList StringToGlyphPlanList(string s)
        {
            m_layout.Layout(s.ToCharArray(), 0, s.Length);
            var l = new UnscaledGlyphPlanList();
            foreach (var g in m_layout.GetUnscaledGlyphPlanIter())
                l.Append(g);
            return l;
        }

        public double GetScale(double point_size)
            => m_openfont.CalculateScaleToPixelFromPointSize((float)point_size);

        public IDictionary<ushort, double> AdvanceWidths { get => m_gtf.AdvanceWidths; }
        public IDictionary<ushort, double> AdvanceHeights { get => m_gtf.AdvanceHeights; }
        public double Height { get => m_gtf.Height; }
        public double Baseline { get => m_gtf.Baseline; }

        public void RenderGlyph(DrawingContext dc, ushort gid, Point origin, double size, Brush fallback_brush)
        {
            ushort layer_index;
            if (m_openfont.COLRTable != null && m_openfont.CPALTable != null
                 && m_openfont.COLRTable.LayerIndices.TryGetValue(gid, out layer_index))
            {
                int start = layer_index, stop = layer_index + m_openfont.COLRTable.LayerCounts[gid];
                int palette = 0; // FIXME: support multiple palettes?

                for (int i = start; i < stop; ++i)
                {
                    ushort sub_gid = m_openfont.COLRTable.GlyphLayers[i];
                    // We do not need to provide advances since we only render
                    // one glyph.
                    GlyphRun r = new GlyphRun(m_gtf, 0, false, size,
                                              new ushort[] { sub_gid },
                                              origin, new double[] { 0 },
                                              null, null, null, // FIXME: check what this is?
                                              null, null, null);
                    int cid = m_openfont.CPALTable.Palettes[palette] + m_openfont.COLRTable.GlyphPalettes[i];
                    byte R, G, B, A;
                    m_openfont.CPALTable.GetColor(cid, out R, out G, out B, out A);
                    Brush b = new SolidColorBrush(Color.FromArgb(A, R, G, B));

                    dc.DrawGlyphRun(b, r);
                }
            }
            else
            {
                GlyphRun r = new GlyphRun(m_gtf, 0, false, size,
                                          new[] { gid },
                                          origin, new double[] { 0 },
                                          null, null, null,
                                          clusterMap: null, caretStops: null, language: null);
                dc.DrawGlyphRun(fallback_brush, r);
            }
        }

        protected GlyphTypeface m_gtf;
        protected Typography.OpenFont.Typeface m_openfont;
        protected GlyphLayout m_layout;
    }
}
