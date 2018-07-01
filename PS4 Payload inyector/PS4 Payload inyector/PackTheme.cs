
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#region Technical Pre-Setup

#region Extensions + Enumerators

public static class SharpTwist
{
    static internal void DrawRoundRectangle(Graphics Graphics, Rectangle Bounds, int Radius, Pen Outline, LinearGradientBrush Fill)
    {
        Bounds = new Rectangle(Bounds.X, Bounds.Y, (Bounds.Width - 1), (Bounds.Height - 1));
        using (GraphicsPath Path = new GraphicsPath(FillMode.Winding))
        {
            Path.AddArc(Bounds.X, Bounds.Y, Radius, Radius, 180f, 90f);
            Path.AddArc(Bounds.Right - Radius, Bounds.Y, Radius, Radius, 270f, 90f);
            Path.AddArc(Bounds.Right - Radius, Bounds.Bottom - Radius, Radius, Radius, 0f, 90f);
            Path.AddArc(Bounds.X, Bounds.Bottom - Radius, Radius, Radius, 90f, 90f);
            Path.CloseFigure();
            Graphics.FillPath(Fill, Path);
            Graphics.DrawPath(Outline, Path);
        }
    }
    static internal void DrawRoundRectangle(Graphics Graphics, Rectangle Bounds, int Radius, Pen Outline, Color Fill)
    {
        Bounds = new Rectangle(Bounds.X, Bounds.Y, (Bounds.Width - 1), (Bounds.Height - 1));
        using (GraphicsPath Path = new GraphicsPath(FillMode.Winding))
        {
            Path.AddArc(Bounds.X, Bounds.Y, Radius, Radius, 180f, 90f);
            Path.AddArc(Bounds.Right - Radius, Bounds.Y, Radius, Radius, 270f, 90f);
            Path.AddArc(Bounds.Right - Radius, Bounds.Bottom - Radius, Radius, Radius, 0f, 90f);
            Path.AddArc(Bounds.X, Bounds.Bottom - Radius, Radius, Radius, 90f, 90f);
            Path.CloseFigure();
            Graphics.FillPath(new SolidBrush(Fill), Path);
            Graphics.DrawPath(Outline, Path);
        }
    }
    static internal void DrawCross(Graphics Graphics, Point Location)
    {
        using (GraphicsPath Path = new GraphicsPath())
        {
            Path.AddLine(Location.X + 1, Location.Y, Location.X + 3, Location.Y);
            Path.AddLine(Location.X + 5, Location.Y + 2, Location.X + 7, Location.Y);
            Path.AddLine(Location.X + 9, Location.Y, Location.X + 10, Location.Y + 1);
            Path.AddLine(Location.X + 7, Location.Y + 4, Location.X + 7, Location.Y + 5);
            Path.AddLine(Location.X + 10, Location.Y + 8, Location.X + 9, Location.Y + 9);
            Path.AddLine(Location.X + 7, Location.Y + 9, Location.X + 5, Location.Y + 7);
            Path.AddLine(Location.X + 3, Location.Y + 9, Location.X + 1, Location.Y + 9);
            Path.AddLine(Location.X + 0, Location.Y + 8, Location.X + 3, Location.Y + 5);
            Path.AddLine(Location.X + 3, Location.Y + 4, Location.X + 0, Location.Y + 1);
            Graphics.FillPath(Brushes.White, Path);
            Graphics.DrawPath(Pens.Black, Path);
        }
    }
    static internal LinearGradientBrush CreateGradient(int Width, int Height, Color Color1, Color Color2) { return new LinearGradientBrush(new Point((Width / 2), 0), new Point((Width / 2), Height), Color1, Color2); }
    static internal void DrawString(Graphics Graphics, Font Font, string Text, Color Color, StringAlignment HorizontalAlignment, PointF Location)
    {
        StringFormat TextFormat = new StringFormat();
        TextFormat.Alignment = HorizontalAlignment;
        Graphics.DrawString(Text, Font, new SolidBrush(Color), Location, TextFormat);
    }
}

[DefaultEvent("Load")]
class STTheme : ContainerControl
{
    #region Properties
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override string Text { get { if (Parent is Form) return ParentForm.Text; else return base.Text; } set { if (Parent is Form) ParentForm.Text = value; base.Text = value; Invalidate(); } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; if (Parent is Form) ParentForm.ForeColor = value; Invalidate(); } }

    protected override bool DoubleBuffered { get { return base.DoubleBuffered; } set { base.DoubleBuffered = value; Invalidate(); } }
    private Point MouseDownPoint = Point.Empty;
    public virtual string Title { get { return Text; } set { Text = value; } }
    public virtual Color TitleColorFront { get { return ForeColor; } set { ForeColor = value; } }
    private Color _TitleColorBack;
    public virtual Color TitleColorBack { get { return _TitleColorBack; } set { _TitleColorBack = value; Invalidate(); } }
    public new Size Size { get { return base.Size; } set { if (Parent is Form) ParentForm.Size = value; base.Size = value; Invalidate(); } }
    private int _Radius = 5;
    public virtual int Radius { get { return _Radius; } set { _Radius = value; Invalidate(); } }
    public new virtual Color BackColor { get { return base.BackColor; } set { base.BackColor = value; Invalidate(); } }

    private FormBorderStyle _BorderStyle = FormBorderStyle.None;
    public virtual FormBorderStyle BorderStyle { get { if (Parent is Form) return ParentForm.FormBorderStyle; else return _BorderStyle; } set { if (Parent is Form) ParentForm.FormBorderStyle = value; _BorderStyle = value; Invalidate(); } }
    private Color _TransparencyKey = Color.Magenta;
    public virtual Color TransparencyKey { get { if (Parent is Form) return ParentForm.TransparencyKey; else return _TransparencyKey; } set { if (Parent is Form) ParentForm.TransparencyKey = value; _TransparencyKey = value; Invalidate(); } }
    private bool _TopMost = false;
    public virtual bool TopMost { get { if (Parent is Form) return ParentForm.TopMost; else return _TopMost; } set { if (Parent is Form) ParentForm.TopMost = value; _TopMost = value; Invalidate(); } }
    private FormStartPosition _StartPosition = FormStartPosition.CenterScreen;
    public FormStartPosition StartPosition { get { if (Parent is Form) return ParentForm.StartPosition; else return _StartPosition; } set { if (Parent is Form) ParentForm.StartPosition = value; _StartPosition = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override RightToLeft RightToLeft { get { return base.RightToLeft; } set { base.RightToLeft = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool UseWaitCursor { get { return base.UseWaitCursor; } set { base.UseWaitCursor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new int TabIndex { get { return base.TabIndex; } set { base.TabIndex = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool TabStop { get { return base.TabStop; } set { base.TabStop = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override AnchorStyles Anchor { get { return base.Anchor; } set { base.Anchor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override bool AutoScroll { get { return base.AutoScroll; } set { base.AutoScroll = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new Size AutoScrollMargin { get { return base.AutoScrollMargin; } set { base.AutoScrollMargin = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new Size AutoScrollMinSize { get { return base.AutoScrollMinSize; } set { base.AutoScrollMinSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new Point Location { get { return base.Location; } set { base.Location = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MinimumSize { get { return base.MinimumSize; } set { base.MinimumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MaximumSize { get { return base.MaximumSize; } set { base.MaximumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new ImeMode ImeMode { get { return base.ImeMode; } set { base.ImeMode = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override DockStyle Dock { get { return base.Dock; } set { base.Dock = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool CausesValidation { get { return base.CausesValidation; } set { base.CausesValidation = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ContextMenuStrip ContextMenuStrip { get { return base.ContextMenuStrip; } set { base.ContextMenuStrip = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool Enabled { get { return base.Enabled; } set { base.Enabled = value; } }
    #endregion

    public STTheme()
    {
        BackColor = Color.FromArgb(50, 50, 50);
        Font = new Font("Verdana", 9, FontStyle.Bold);
        TitleColorFront = Color.White;
        TitleColorBack = Color.Black;
        DoubleBuffered = true;
    }

    protected override void OnParentChanged(System.EventArgs e)
    {
        if (!(Parent is Form)) return;
        ParentForm.Load += new System.EventHandler(Load);
        BorderStyle = FormBorderStyle.None;
        ParentForm.BackColor = Color.Magenta;
        TransparencyKey = ParentForm.BackColor;
        Title = ParentForm.Text;
        base.OnParentChanged(e);
    }

    void Load(object sender, System.EventArgs e) { }

    protected override void OnResize(System.EventArgs e) { if (Parent is Form) { Location = new Point(0, 0); ParentForm.Size = Size; } base.OnResize(e); }
    protected override void OnMove(System.EventArgs e) { if (Parent is Form) { Location = new Point(0, 0); ParentForm.Size = Size; } base.OnMove(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(TransparencyKey);
        SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 0, Width, Height), Radius, new Pen(Color.Black, 1), BackColor);
        SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 0, Width, 27), (Radius / 2), new Pen(Color.Black, 1), BackColor);
        SharpTwist.DrawString(e.Graphics, Font, Title, TitleColorBack, StringAlignment.Near, new PointF(6, 6));
        SharpTwist.DrawString(e.Graphics, Font, Title, TitleColorFront, StringAlignment.Near, new PointF(5, 5));
    }

    protected override void OnMouseDown(MouseEventArgs e) { if (e.Button != MouseButtons.Left) return; MouseDownPoint = new Point(e.X, e.Y); }
    protected override void OnMouseMove(MouseEventArgs e) { if ((!(Parent is Form)) || (MouseDownPoint == Point.Empty)) return; ParentForm.Location = new Point((ParentForm.Left + e.X - MouseDownPoint.X), (ParentForm.Top + e.Y - MouseDownPoint.Y)); }
    protected override void OnMouseUp(MouseEventArgs e) { if (e.Button != MouseButtons.Left) return; MouseDownPoint = Point.Empty; }
}

[DefaultEvent("TextChanged")]
class STTextBox : Control
{
    #region Properties
    private TextBox Base;
    public override string Text { get { return base.Text; } set { base.Text = value; Base.Text = value; Invalidate(); } }
    public override Font Font { get { return base.Font; } set { base.Font = value; Base.Font = value; Invalidate(); } }
    private Color _BackColorTop, _BackColorBottom;
    public virtual Color BackColorTop { get { return _BackColorTop; } set { _BackColorTop = value; Invalidate(); } }
    public virtual Color BackColorBottom { get { return _BackColorBottom; } set { _BackColorBottom = value; Invalidate(); } }
    private HorizontalAlignment _HorizontalAlignment = HorizontalAlignment.Left;
    public virtual HorizontalAlignment HorizontalAlignment { get { return _HorizontalAlignment; } set { _HorizontalAlignment = value; Base.TextAlign = value; Invalidate(); } }
    public override Color BackColor { get { return base.BackColor; } set { base.BackColor = value; Base.BackColor = value; Invalidate(); } }
    public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; Base.ForeColor = value; Invalidate(); } }
    private Color _OutlineColor = Color.White;
    public virtual Color OutlineColor { get { return _OutlineColor; } set { _OutlineColor = value; Invalidate(); } }
    private char _PasswordChar;
    public virtual char PasswordChar { get { return _PasswordChar; } set { _PasswordChar = value; Base.PasswordChar = value; Invalidate(); } }
    private int _Radius = 3;
    public virtual int Radius { get { return _Radius; } set { _Radius = value; Invalidate(); } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override RightToLeft RightToLeft { get { return base.RightToLeft; } set { base.RightToLeft = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool UseWaitCursor { get { return base.UseWaitCursor; } set { base.UseWaitCursor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MinimumSize { get { return base.MinimumSize; } set { base.MinimumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MaximumSize { get { return base.MaximumSize; } set { base.MaximumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new ImeMode ImeMode { get { return base.ImeMode; } set { base.ImeMode = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool CausesValidation { get { return base.CausesValidation; } set { base.CausesValidation = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ContextMenuStrip ContextMenuStrip { get { return base.ContextMenuStrip; } set { base.ContextMenuStrip = value; } }
    #endregion

    public STTextBox()
    {
        Cursor = Cursors.IBeam;
        Base = new TextBox();
        Base.TextChanged += new System.EventHandler(Base_TextChanged);
        Base.KeyDown += new KeyEventHandler(Base_KeyDown);
        Base.BorderStyle = BorderStyle.None;
        Font = new Font("Verdana", 8, FontStyle.Regular);
        ForeColor = Color.White;
        MinimumSize = new Size(40, 21); Size = new Size(120, MinimumSize.Height);
        OutlineColor = Color.FromArgb(30, 30, 30);
        BackColorTop = Color.FromArgb(60, 60, 60);
        BackColor = Color.FromArgb(50, 50, 50);
        BackColorBottom = Color.FromArgb(40, 40, 40);
    }

    protected override void OnHandleCreated(System.EventArgs e) { if (!Controls.Contains(Base)) Controls.Add(Base); base.OnHandleCreated(e); }
    void Base_KeyDown(object sender, KeyEventArgs e) { if ((e.Control) && (e.KeyCode == Keys.A)) { Base.SelectAll(); e.SuppressKeyPress = true; } }
    void Base_TextChanged(object sender, System.EventArgs e) { Text = Base.Text; }
    protected override void OnResize(System.EventArgs e) { Base.Location = new Point(4, 4); Base.Size = new Size((Width - 8), (Height - 8)); base.OnResize(e); }
    protected override void OnMouseDown(MouseEventArgs e) { Base.Focus(); base.OnMouseDown(e); }
    protected override void OnEnter(System.EventArgs e) { Base.Focus(); Invalidate(); base.OnEnter(e); }
    protected override void OnLeave(System.EventArgs e) { Invalidate(); base.OnLeave(e); }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent.BackColor);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 0, Width, Height), Radius, new Pen(OutlineColor, 1), SharpTwist.CreateGradient(Width, Height, BackColorTop, BackColorBottom));
    }
}

class STComboBox : ComboBox
{
    #region Properties
    private Color _BackColorTop, _BackColorBottom, _OutlineColor, _TextColorBack;
    public virtual Color BackColorTop { get { return _BackColorTop; } set { _BackColorTop = value; Invalidate(); } }
    public virtual Color BackColorBottom { get { return _BackColorBottom; } set { _BackColorBottom = value; Invalidate(); } }
    public virtual Color OutlineColor { get { return _OutlineColor; } set { _OutlineColor = value; Invalidate(); } }
    public virtual Color TextColor { get { return ForeColor; } set { ForeColor = value; Invalidate(); } }
    public virtual Color TextColorBack { get { return _TextColorBack; } set { _TextColorBack = value; Invalidate(); } }
    private int _Radius = 3;
    public virtual int Radius { get { return _Radius; } set { _Radius = value; Invalidate(); } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override RightToLeft RightToLeft { get { return base.RightToLeft; } set { base.RightToLeft = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool UseWaitCursor { get { return base.UseWaitCursor; } set { base.UseWaitCursor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override AnchorStyles Anchor { get { return base.Anchor; } set { base.Anchor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MinimumSize { get { return base.MinimumSize; } set { base.MinimumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MaximumSize { get { return base.MaximumSize; } set { base.MaximumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new ImeMode ImeMode { get { return base.ImeMode; } set { base.ImeMode = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool CausesValidation { get { return base.CausesValidation; } set { base.CausesValidation = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ContextMenuStrip ContextMenuStrip { get { return base.ContextMenuStrip; } set { base.ContextMenuStrip = value; } }
    #endregion

    public STComboBox()
    {
        SetStyle((ControlStyles)139286, true);
        SetStyle(ControlStyles.Selectable, false);
        BackColorTop = Color.FromArgb(60, 60, 60);
        BackColorBottom = Color.FromArgb(40, 40, 40);
        DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        DropDownStyle = ComboBoxStyle.DropDownList;
        OutlineColor = Color.FromArgb(30, 30, 30);
        TextColor = Color.White;
        TextColorBack = Color.Black;
        Size = new Size(140, Height);
        Font = new Font("Verdana", 8, FontStyle.Regular);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if ((SelectedIndex == -1) && (Items.Count > 0)) { SelectedIndex = 0; Text = Items[0].ToString(); Invalidate(); }
        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        e.Graphics.Clear(Parent.BackColor);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 0, Width, Height), Radius, new Pen(OutlineColor, 1), SharpTwist.CreateGradient(Width, Height, BackColorTop, BackColorBottom));
        SizeF TextSize = e.Graphics.MeasureString(Text, Font);
        SharpTwist.DrawString(e.Graphics, Font, Text, TextColorBack, StringAlignment.Near, new PointF(5, ((Height / 2) - (TextSize.Height / 2) + 1)));
        SharpTwist.DrawString(e.Graphics, Font, Text, TextColor, StringAlignment.Near, new PointF(4, ((Height / 2) - (TextSize.Height / 2))));
        e.Graphics.DrawLine(new Pen(Brushes.Black, 2f), Width - 15, 10, Width - 11, 13);
        e.Graphics.DrawLine(new Pen(Brushes.Black, 2f), Width - 7, 10, Width - 11, 13);
        e.Graphics.DrawLine(Pens.Black, Width - 11, 13, Width - 11, 14);
        e.Graphics.DrawLine(new Pen(Color.White, 2f), Width - 16, 9, Width - 12, 12);
        e.Graphics.DrawLine(new Pen(Color.White, 2f), Width - 8, 9, Width - 12, 12);
        e.Graphics.DrawLine(Pens.White, Width - 12, 12, Width - 12, 13);
        e.Graphics.DrawLine(new Pen(Color.FromArgb(35, 35, 35)), Width - 22, 0, Width - 22, Height);
        e.Graphics.DrawLine(new Pen(Color.FromArgb(65, 65, 65)), (Width - 23), 2, (Width - 23), (Height - 4));
        e.Graphics.DrawLine(new Pen(Color.FromArgb(65, 65, 65)), (Width - 21), 2, (Width - 21), (Height - 4));
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) e.Graphics.FillRectangle(new SolidBrush(BackColorTop), e.Bounds);
        else e.Graphics.FillRectangle(new SolidBrush(BackColorBottom), e.Bounds);
        if (!(e.Index == -1)) e.Graphics.DrawString(GetItemText(Items[e.Index]), e.Font, new SolidBrush(TextColor), e.Bounds);
    }
}

class STControlButton : Control
{
    #region Properties
    public enum Button { Minimize, Maximize, Close }
    private Button _ControlButton;
    public Button ControlButton { get { return _ControlButton; } set { _ControlButton = value; Invalidate(); } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override RightToLeft RightToLeft { get { return base.RightToLeft; } set { base.RightToLeft = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool UseWaitCursor { get { return base.UseWaitCursor; } set { base.UseWaitCursor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new int TabIndex { get { return base.TabIndex; } set { base.TabIndex = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool TabStop { get { return base.TabStop; } set { base.TabStop = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MinimumSize { get { return base.MinimumSize; } set { base.MinimumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MaximumSize { get { return base.MaximumSize; } set { base.MaximumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new ImeMode ImeMode { get { return base.ImeMode; } set { base.ImeMode = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override DockStyle Dock { get { return base.Dock; } set { base.Dock = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool CausesValidation { get { return base.CausesValidation; } set { base.CausesValidation = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ContextMenuStrip ContextMenuStrip { get { return base.ContextMenuStrip; } set { base.ContextMenuStrip = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool Enabled { get { return base.Enabled; } set { base.Enabled = value; } }
    #endregion

    public STControlButton()
    {
        Anchor = AnchorStyles.Top | AnchorStyles.Right;
        ControlButton = Button.Close;
        Size = new Size(18, 20);
        MinimumSize = Size;
        MaximumSize = Size;
        Margin = new Padding(0);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent.BackColor);
        switch (ControlButton)
        {
            case Button.Close: SharpTwist.DrawCross(e.Graphics, new Point(4, 5)); break;
            case Button.Maximize: if (FindForm().WindowState == FormWindowState.Maximized) DrawRestore(e.Graphics, new Point(3, 4)); else DrawMaximize(e.Graphics, new Point(3, 5)); break;
            case Button.Minimize: DrawMinimize(e.Graphics, new Point(3, 10)); break;
        }
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
            Form ParentForm = FindForm();
            switch (ControlButton)
            {
                case Button.Minimize: ParentForm.WindowState = FormWindowState.Minimized; break;
                case Button.Maximize: if (ParentForm.WindowState == FormWindowState.Normal) ParentForm.WindowState = FormWindowState.Maximized; else ParentForm.WindowState = FormWindowState.Normal; break;
                case Button.Close: ParentForm.Close(); break;
            }
        }
        Invalidate();
        base.OnMouseClick(e);
    }

    private void DrawMinimize(Graphics Graphics, Point Location)
    {
        Graphics.FillRectangle(Brushes.White, Location.X, Location.Y, 12, 5);
        Graphics.DrawRectangle(Pens.Black, Location.X, Location.Y, 11, 4);
    }
    private void DrawMaximize(Graphics Graphics, Point Location)
    {
        Graphics.DrawRectangle(new Pen(Color.White, 2), Location.X + 2, Location.Y + 2, 8, 6);
        Graphics.DrawRectangle(Pens.Black, Location.X, Location.Y, 11, 9);
        Graphics.DrawRectangle(Pens.Black, Location.X + 3, Location.Y + 3, 5, 3);
    }
    private void DrawRestore(Graphics Graphics, Point Location)
    {
        Graphics.FillRectangle(Brushes.White, Location.X + 3, Location.Y + 1, 8, 4);
        Graphics.FillRectangle(Brushes.White, Location.X + 7, Location.Y + 5, 4, 4);
        Graphics.DrawRectangle(Pens.Black, Location.X + 2, Location.Y + 0, 9, 9);

        Graphics.FillRectangle(Brushes.White, Location.X + 1, Location.Y + 3, 2, 6);
        Graphics.FillRectangle(Brushes.White, Location.X + 1, Location.Y + 9, 8, 2);
        Graphics.DrawRectangle(Pens.Black, Location.X, Location.Y + 2, 9, 9);
        Graphics.DrawRectangle(Pens.Black, Location.X + 3, Location.Y + 5, 3, 3);
    }
}

class STLabel : Control
{
    #region Properties
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Color BackColor { get { return base.BackColor; } set { base.BackColor = value; } }

    private Color _TextColorBack;
    public virtual Color TextColor { get { return ForeColor; } set { ForeColor = value; Invalidate(); } }
    public virtual Color TextColorBack { get { return _TextColorBack; } set { _TextColorBack = value; Invalidate(); } }
    public override string Text { get { return base.Text; } set { base.Text = value; Invalidate(); } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override RightToLeft RightToLeft { get { return base.RightToLeft; } set { base.RightToLeft = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool UseWaitCursor { get { return base.UseWaitCursor; } set { base.UseWaitCursor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new int TabIndex { get { return base.TabIndex; } set { base.TabIndex = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool TabStop { get { return base.TabStop; } set { base.TabStop = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MinimumSize { get { return base.MinimumSize; } set { base.MinimumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MaximumSize { get { return base.MaximumSize; } set { base.MaximumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new ImeMode ImeMode { get { return base.ImeMode; } set { base.ImeMode = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override DockStyle Dock { get { return base.Dock; } set { base.Dock = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool CausesValidation { get { return base.CausesValidation; } set { base.CausesValidation = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ContextMenuStrip ContextMenuStrip { get { return base.ContextMenuStrip; } set { base.ContextMenuStrip = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool Enabled { get { return base.Enabled; } set { base.Enabled = value; } }
    #endregion

    public STLabel()
    {
        TextColor = Color.White;
        TextColorBack = Color.Black;
        Font = new Font("Verdana", 8, FontStyle.Regular);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        SizeF TextSize = e.Graphics.MeasureString(Text, Font);
        Size = new Size((int)TextSize.Width, (int)TextSize.Height);
        e.Graphics.Clear(Parent.BackColor);
        SharpTwist.DrawString(e.Graphics, Font, Text, TextColorBack, StringAlignment.Near, new PointF(1, 1));
        SharpTwist.DrawString(e.Graphics, Font, Text, TextColor, StringAlignment.Near, new PointF(0, 0));
    }
}

[DefaultEvent("CheckedChanged")]
class STRadioButton : Control
{
    #region Properties
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Color BackColor { get { return base.BackColor; } set { base.BackColor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; } }

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);
    private int _BorderThickness = 2;
    public virtual int BorderThickness { get { return _BorderThickness; } set { _BorderThickness = value; Invalidate(); } }
    private bool _Checked;
    public bool Checked { get { return _Checked; } set { _Checked = value; if (value) InvalidateParent(); if (CheckedChanged != null) CheckedChanged(this); Invalidate(); } }
    public override string Text { get { return base.Text; } set { base.Text = value; Invalidate(); } }
    public virtual Color TextColor { get { return ForeColor; } set { ForeColor = value; Invalidate(); } }
    public virtual Color TextColorBack { get { return _TextColorBack; } set { _TextColorBack = value; Invalidate(); } }
    private Color _BackColorTop, _BackColorBottom, _TextColorBack, _OutlineColor, _CheckedColor;
    public virtual Color CheckedColor { get { return _CheckedColor; } set { _CheckedColor = value; Invalidate(); } }
    public virtual Color OutlineColor { get { return _OutlineColor; } set { _OutlineColor = value; Invalidate(); } }
    public virtual Color BackColorTop { get { return _BackColorTop; } set { _BackColorTop = value; Invalidate(); } }
    public virtual Color BackColorBottom { get { return _BackColorBottom; } set { _BackColorBottom = value; Invalidate(); } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override RightToLeft RightToLeft { get { return base.RightToLeft; } set { base.RightToLeft = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool UseWaitCursor { get { return base.UseWaitCursor; } set { base.UseWaitCursor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MinimumSize { get { return base.MinimumSize; } set { base.MinimumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MaximumSize { get { return base.MaximumSize; } set { base.MaximumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new ImeMode ImeMode { get { return base.ImeMode; } set { base.ImeMode = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override DockStyle Dock { get { return base.Dock; } set { base.Dock = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool CausesValidation { get { return base.CausesValidation; } set { base.CausesValidation = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ContextMenuStrip ContextMenuStrip { get { return base.ContextMenuStrip; } set { base.ContextMenuStrip = value; } }
    #endregion

    public STRadioButton()
    {
        TextColor = Color.White;
        TextColorBack = Color.Black;
        OutlineColor = Color.FromArgb(40, 40, 40);
        CheckedColor = Color.White;
        Font = new Font("Verdana", 8, FontStyle.Regular);
        MinimumSize = new Size(Width, 17); Size = MinimumSize;
    }

    private void InvalidateParent()
    {
        if (Parent == null) return;
        foreach (Control Control in Parent.Controls) if ((!ReferenceEquals(Control, this)) && (Control is STRadioButton)) (Control as STRadioButton).Checked = false;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent.BackColor);
        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle Bounds = new Rectangle(3, 2, (Height - 5), (Height - 5));
        GraphicsPath Path = new GraphicsPath();
        Path.AddEllipse(Bounds);
        PathGradientBrush Brush = new PathGradientBrush(Path);
        Brush.CenterColor = BackColorTop;
        Brush.SurroundColors = new Color[] { BackColorBottom };
        Brush.FocusScales = new PointF(.3f, .3f);
        e.Graphics.FillPath(Brush, Path);
        e.Graphics.DrawEllipse(new Pen(OutlineColor, BorderThickness), Bounds);
        if (Checked)
        {
            Rectangle CheckedBounds = new Rectangle((Bounds.X + 4), (Bounds.Y + 4), (Bounds.Height - 8), (Bounds.Height - 8));
            e.Graphics.FillEllipse(new SolidBrush(CheckedColor), CheckedBounds);
            e.Graphics.DrawEllipse(new Pen(CheckedColor, BorderThickness), CheckedBounds);
        }
        SizeF TextSize = e.Graphics.MeasureString(Text, Font);
        Size = new Size(((int)TextSize.Width + Height), Height);
        Point TextLocation = new Point(Height, ((Height / 2) - (int)(TextSize.Height / 2)));
        SharpTwist.DrawString(e.Graphics, Font, Text, TextColorBack, StringAlignment.Near, new PointF((TextLocation.X + 1), (TextLocation.Y + 1)));
        SharpTwist.DrawString(e.Graphics, Font, Text, TextColor, StringAlignment.Near, new PointF(TextLocation.X, TextLocation.Y));
    }

    protected override void OnMouseDown(MouseEventArgs e) { Checked = true; base.OnMouseDown(e); }
}

[DefaultEvent("CheckedChanged")]
class STCheckBox : Control
{
    #region Properties
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Color BackColor { get { return base.BackColor; } set { base.BackColor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; } }

    public event CheckedChangedEventHandler CheckedChanged;
    public delegate void CheckedChangedEventHandler(object sender);
    public enum CheckedTypes { Blop, Cross, Tick }
    private CheckedTypes _CheckedType = CheckedTypes.Blop;
    private int _BorderThickness = 2;
    public virtual int BorderThickness { get { return _BorderThickness; } set { _BorderThickness = value; Invalidate(); } }
    public virtual CheckedTypes CheckedType { get { return _CheckedType; } set { _CheckedType = value; Invalidate(); } }
    private bool _Checked;
    public bool Checked { get { return _Checked; } set { _Checked = value; if (CheckedChanged != null) CheckedChanged(this); Invalidate(); } }
    public override string Text { get { return base.Text; } set { base.Text = value; Invalidate(); } }
    public virtual Color TextColor { get { return ForeColor; } set { ForeColor = value; Invalidate(); } }
    public virtual Color TextColorBack { get { return _TextColorBack; } set { _TextColorBack = value; Invalidate(); } }
    private Color _BackColorTop, _BackColorBottom, _TextColorBack, _OutlineColor, _CheckedColor, _CheckedColorBack;
    public virtual Color CheckedColor { get { return _CheckedColor; } set { _CheckedColor = value; Invalidate(); } }
    public virtual Color CheckedColorBack { get { return _CheckedColorBack; } set { _CheckedColorBack = value; Invalidate(); } }
    public virtual Color OutlineColor { get { return _OutlineColor; } set { _OutlineColor = value; Invalidate(); } }
    public virtual Color BackColorTop { get { return _BackColorTop; } set { _BackColorTop = value; Invalidate(); } }
    public virtual Color BackColorBottom { get { return _BackColorBottom; } set { _BackColorBottom = value; Invalidate(); } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override RightToLeft RightToLeft { get { return base.RightToLeft; } set { base.RightToLeft = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool UseWaitCursor { get { return base.UseWaitCursor; } set { base.UseWaitCursor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MinimumSize { get { return base.MinimumSize; } set { base.MinimumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MaximumSize { get { return base.MaximumSize; } set { base.MaximumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new ImeMode ImeMode { get { return base.ImeMode; } set { base.ImeMode = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override DockStyle Dock { get { return base.Dock; } set { base.Dock = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool CausesValidation { get { return base.CausesValidation; } set { base.CausesValidation = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ContextMenuStrip ContextMenuStrip { get { return base.ContextMenuStrip; } set { base.ContextMenuStrip = value; } }
    #endregion

    public STCheckBox()
    {
        TextColor = Color.White;
        TextColorBack = Color.Black;
        OutlineColor = Color.FromArgb(40, 40, 40);
        CheckedColor = Color.White;
        CheckedColorBack = Color.Black;
        Font = new Font("Verdana", 8, FontStyle.Regular);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent.BackColor);
        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle Bounds = new Rectangle(3, 2, (Height - 5), (Height - 5));
        SharpTwist.DrawRoundRectangle(e.Graphics, Bounds, 1, new Pen(OutlineColor, BorderThickness), SharpTwist.CreateGradient(Width, Height, BackColorTop, BackColorBottom));
        if (Checked)
        {
            Rectangle CheckedBounds = new Rectangle((Bounds.X + 2), (Bounds.Y + 2), (Bounds.Height - 4), (Bounds.Height - 4));
            switch (CheckedType)
            {
                case CheckedTypes.Blop: SharpTwist.DrawRoundRectangle(e.Graphics, CheckedBounds, 1, new Pen(OutlineColor, BorderThickness), CheckedColor); break;
                case CheckedTypes.Cross:
                    using (GraphicsPath Path = new GraphicsPath())
                    {
                        Path.AddLine(CheckedBounds.X, CheckedBounds.Y - 1, CheckedBounds.X + 2, CheckedBounds.Y - 1);
                        Path.AddLine(CheckedBounds.X + 4, CheckedBounds.Y + 1, CheckedBounds.X + 6, CheckedBounds.Y - 1);
                        Path.AddLine(CheckedBounds.X + 8, CheckedBounds.Y - 1, CheckedBounds.X + 9, CheckedBounds.Y);
                        Path.AddLine(CheckedBounds.X + 6, CheckedBounds.Y + 3, CheckedBounds.X + 6, CheckedBounds.Y + 4);
                        Path.AddLine(CheckedBounds.X + 9, CheckedBounds.Y + 7, CheckedBounds.X + 8, CheckedBounds.Y + 8);
                        Path.AddLine(CheckedBounds.X + 6, CheckedBounds.Y + 8, CheckedBounds.X + 4, CheckedBounds.Y + 6);
                        Path.AddLine(CheckedBounds.X + 2, CheckedBounds.Y + 8, CheckedBounds.X, CheckedBounds.Y + 8);
                        Path.AddLine(CheckedBounds.X - 1, CheckedBounds.Y + 7, CheckedBounds.X + 2, CheckedBounds.Y + 4);
                        Path.AddLine(CheckedBounds.X + 2, CheckedBounds.Y + 3, CheckedBounds.X - 1, CheckedBounds.Y);
                        e.Graphics.FillPath(Brushes.White, Path);
                        e.Graphics.DrawPath(Pens.Black, Path);
                    }
                    break;
                case CheckedTypes.Tick:
                    e.Graphics.DrawLine(new Pen(CheckedColorBack, BorderThickness), 6, (Height - 6), 9, (Height - 7));
                    e.Graphics.DrawLine(new Pen(CheckedColorBack, BorderThickness), 8, (Height - 4), (Height - 3), 6);
                    e.Graphics.DrawLine(new Pen(CheckedColor, BorderThickness), 5, (Height - 8), 8, (Height - 6));
                    e.Graphics.DrawLine(new Pen(CheckedColor, BorderThickness), 7, (Height - 6), (Height - 5), 4);
                    break;
            }
        }
        SizeF TextSize = e.Graphics.MeasureString(Text, Font);
        Size = new Size(((int)TextSize.Width + Height), Height);
        Point TextLocation = new Point(Height, ((Height / 2) - (int)(TextSize.Height / 2)));
        SharpTwist.DrawString(e.Graphics, Font, Text, TextColorBack, StringAlignment.Near, new PointF((TextLocation.X + 1), (TextLocation.Y + 1)));
        SharpTwist.DrawString(e.Graphics, Font, Text, TextColor, StringAlignment.Near, new PointF(TextLocation.X, TextLocation.Y));
    }

    protected override void OnMouseClick(MouseEventArgs e) { Checked = !Checked; base.OnMouseClick(e); }
}

class STProgressBar : Control
{
    #region Properties
    private int _Minimum = 0, _Value = 50, _Maximum = 100;
    public virtual int Minimum { get { return _Minimum; } set { if ((value >= 0) && (value < Maximum)) { _Minimum = value; Invalidate(); } } }
    public virtual int Value { get { return _Value; } set { if (value < Minimum) value = Minimum; else if (value > Maximum) value = Maximum; _Value = value; Invalidate(); } }
    public virtual int Maximum { get { return _Maximum; } set { if (value <= Minimum) value = (Minimum + 1); _Maximum = value; Invalidate(); } }
    public enum ProgressTexts { None, Flow, Bubble, AlignLeft, AlignCenter, AlignRight }
    private ProgressTexts _ProgressText = ProgressTexts.None;
    public ProgressTexts ProgressText { get { return _ProgressText; } set { _ProgressText = value; Invalidate(); } }
    private Color _BackColorTop, _BackColorBottom, _OutlineColor, _TextColorBack, _ProgressColor, _ProgressColorBack, _ProgressOutlineColor, _ProgressTextColor, _ProgressTextBackColor;
    public virtual Color ProgressTextColor { get { return _ProgressTextColor; } set { _ProgressTextColor = value; Invalidate(); } }
    public virtual Color ProgressTextBackColor { get { return _ProgressTextBackColor; } set { _ProgressTextBackColor = value; Invalidate(); } }
    public virtual Color ProgressOutlineColor { get { return _ProgressOutlineColor; } set { _ProgressOutlineColor = value; Invalidate(); } }
    public virtual Color ProgressColor { get { return _ProgressColor; } set { _ProgressColor = value; Invalidate(); } }
    public virtual Color ProgressColorBack { get { return _ProgressColorBack; } set { _ProgressColorBack = value; Invalidate(); } }
    public virtual Color BackColorTop { get { return _BackColorTop; } set { _BackColorTop = value; Invalidate(); } }
    public virtual Color BackColorBottom { get { return _BackColorBottom; } set { _BackColorBottom = value; Invalidate(); } }
    public virtual Color OutlineColor { get { return _OutlineColor; } set { _OutlineColor = value; Invalidate(); } }
    public virtual Color TextColor { get { return ForeColor; } set { ForeColor = value; Invalidate(); } }
    public virtual Color TextColorBack { get { return _TextColorBack; } set { _TextColorBack = value; Invalidate(); } }
    private int _BorderThickness = 1;
    public virtual int BorderThickness { get { return _BorderThickness; } set { _BorderThickness = value; Invalidate(); } }
    private int _Radius = 3;
    public virtual int Radius { get { return _Radius; } set { _Radius = value; Invalidate(); } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override RightToLeft RightToLeft { get { return base.RightToLeft; } set { base.RightToLeft = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool UseWaitCursor { get { return base.UseWaitCursor; } set { base.UseWaitCursor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new int TabIndex { get { return base.TabIndex; } set { base.TabIndex = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool TabStop { get { return base.TabStop; } set { base.TabStop = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new ImeMode ImeMode { get { return base.ImeMode; } set { base.ImeMode = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool CausesValidation { get { return base.CausesValidation; } set { base.CausesValidation = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ContextMenuStrip ContextMenuStrip { get { return base.ContextMenuStrip; } set { base.ContextMenuStrip = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool Enabled { get { return base.Enabled; } set { base.Enabled = value; } }
    #endregion

    public STProgressBar()
    {
        MinimumSize = new Size(100, 10);
        Size = new Size(200, 15);
        Font = new Font("Verdana", 6, FontStyle.Bold);
        BackColorBottom = Color.FromArgb(40, 40, 40);
        BackColorTop = Color.FromArgb(60, 60, 60);
        OutlineColor = Color.FromArgb(20, 20, 20);
        ProgressColor = Color.SkyBlue;
        ProgressColorBack = Color.DeepSkyBlue;
        ProgressOutlineColor = Color.DeepSkyBlue;
        ProgressTextColor = Color.White;
        ProgressTextBackColor = Color.Black;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent.BackColor);
        SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 0, Width, Height), Radius, new Pen(OutlineColor, BorderThickness), SharpTwist.CreateGradient(Width, Height, BackColorTop, BackColorBottom));
        int ProgressWidth = (int)(((float)Value / (float)Maximum) * (float)Width);
        SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(3, 3, (ProgressWidth - 6), (Height - 6)), Radius, new Pen(ProgressOutlineColor, BorderThickness), SharpTwist.CreateGradient(Width, Height, ProgressColor, ProgressColorBack));
        string ProgressTextS = (((int)(((float)Value / (float)Maximum) * 100f)).ToString() + "%");
        int ProgressTextWidth = (int)(e.Graphics.MeasureString(ProgressTextS, Font).Width);
        switch (ProgressText)
        {
            case ProgressTexts.Flow:
                int ProgressTextX = (Math.Min((Width - ProgressTextWidth), ProgressWidth));
                SharpTwist.DrawString(e.Graphics, Font, ProgressTextS, ProgressTextBackColor, StringAlignment.Near, new PointF((ProgressTextX + 1), (((Height / 2) - (e.Graphics.MeasureString(ProgressTextS, Font).Height / 2)) + 1)));
                SharpTwist.DrawString(e.Graphics, Font, ProgressTextS, ProgressTextColor, StringAlignment.Near, new PointF(ProgressTextX, ((Height / 2) - (e.Graphics.MeasureString(ProgressTextS, Font).Height / 2))));
                break;
        }
    }
}

class STGroupBox : ContainerControl
{
    #region Properties
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override string Text { get { return base.Text; } set { base.Text = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Font Font { get { return base.Font; } set { base.Font = value; } }

    private string _Title, _Description;
    public virtual string Title { get { return _Title; } set { _Title = value; Invalidate(); } }
    public virtual string Description { get { return _Description; } set { _Description = value; Invalidate(); } }
    private Color _TitleColor, _TitleBackColor, _DescriptionColor, _DescriptionBackColor, _OutlineColor, _SeperatorColor;
    public virtual Color TitleColor { get { return _TitleColor; } set { _TitleColor = value; Invalidate(); } }
    public virtual Color TitleBackColor { get { return _TitleBackColor; } set { _TitleBackColor = value; Invalidate(); } }
    public virtual Color DescriptionColor { get { return _DescriptionColor; } set { _DescriptionColor = value; Invalidate(); } }
    public virtual Color DescriptionBackColor { get { return _DescriptionBackColor; } set { _DescriptionBackColor = value; Invalidate(); } }
    public virtual Color OutlineColor { get { return _OutlineColor; } set { _OutlineColor = value; Invalidate(); } }
    public virtual Color SeperatorColor { get { return _SeperatorColor; } set { _SeperatorColor = value; Invalidate(); } }
    private Font _TitleFont, _DescriptionFont;
    public virtual Font TitleFont { get { return _TitleFont; } set { _TitleFont = value; Invalidate(); } }
    public virtual Font DescriptionFont { get { return _DescriptionFont; } set { _DescriptionFont = value; Invalidate(); } }
    private int _Radius = 3, _OutlineThickness = 1, _SeperatorThickness = 2;
    public virtual int Radius { get { return _Radius; } set { _Radius = value; Invalidate(); } }
    public virtual int OutlineThickness { get { return _OutlineThickness; } set { _OutlineThickness = value; Invalidate(); } }
    public virtual int SeperatorThickness { get { return _SeperatorThickness; } set { _SeperatorThickness = value; Invalidate(); } }
    public enum DescriptionAligns { NextTo, Underneath }
    private DescriptionAligns _DescriptionAlign = DescriptionAligns.Underneath;
    public virtual DescriptionAligns DescriptionAlign { get { return _DescriptionAlign; } set { _DescriptionAlign = value; Invalidate(); } }
    private bool _DrawSeperator = true;
    public virtual bool DrawSeperator { get { return _DrawSeperator; } set { _DrawSeperator = value; Invalidate(); } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override RightToLeft RightToLeft { get { return base.RightToLeft; } set { base.RightToLeft = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool UseWaitCursor { get { return base.UseWaitCursor; } set { base.UseWaitCursor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new int TabIndex { get { return base.TabIndex; } set { base.TabIndex = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool TabStop { get { return base.TabStop; } set { base.TabStop = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override bool AutoScroll { get { return base.AutoScroll; } set { base.AutoScroll = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new Size AutoScrollMargin { get { return base.AutoScrollMargin; } set { base.AutoScrollMargin = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new Size AutoScrollMinSize { get { return base.AutoScrollMinSize; } set { base.AutoScrollMinSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new ImeMode ImeMode { get { return base.ImeMode; } set { base.ImeMode = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool CausesValidation { get { return base.CausesValidation; } set { base.CausesValidation = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ContextMenuStrip ContextMenuStrip { get { return base.ContextMenuStrip; } set { base.ContextMenuStrip = value; } }
    #endregion

    public STGroupBox()
    {
        Title = "Group Title";
        Description = "Group Description";
        TitleFont = new Font("Verdana", 9, FontStyle.Bold);
        DescriptionFont = new Font("Verdana", 6, FontStyle.Italic);
        TitleColor = Color.DeepSkyBlue;
        DescriptionColor = Color.White;
        TitleBackColor = Color.Black;
        DescriptionBackColor = Color.Black;
        OutlineColor = Color.FromArgb(30, 30, 30);
        SeperatorColor = Color.FromArgb(43, 43, 43);
        Size = new Size(300, 300);
    }

    protected override void OnParentChanged(EventArgs e)
    {
        BackColor = Parent.BackColor;
        base.OnParentChanged(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent.BackColor);
        SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 0, Width, Height), Radius, new Pen(OutlineColor, OutlineThickness), BackColor);
        SizeF TitleSize = e.Graphics.MeasureString(Title, TitleFont);
        SharpTwist.DrawString(e.Graphics, TitleFont, Title, TitleBackColor, StringAlignment.Near, new PointF(3, 3));
        SharpTwist.DrawString(e.Graphics, TitleFont, Title, TitleColor, StringAlignment.Near, new PointF(2, 2));
        SizeF DescriptionSize = e.Graphics.MeasureString(Description, DescriptionFont);
        switch (DescriptionAlign)
        {
            case DescriptionAligns.NextTo:
                SharpTwist.DrawString(e.Graphics, DescriptionFont, Description, DescriptionBackColor, StringAlignment.Near, new PointF((3 + TitleSize.Width), (3 + ((TitleSize.Height / 2) - (DescriptionSize.Height / 2)))));
                SharpTwist.DrawString(e.Graphics, DescriptionFont, Description, DescriptionColor, StringAlignment.Near, new PointF((2 + TitleSize.Width), (2 + ((TitleSize.Height / 2) - (DescriptionSize.Height / 2)))));
                break;
            case DescriptionAligns.Underneath:
                SharpTwist.DrawString(e.Graphics, DescriptionFont, Description, DescriptionBackColor, StringAlignment.Near, new PointF(5, (3 + TitleSize.Height)));
                SharpTwist.DrawString(e.Graphics, DescriptionFont, Description, DescriptionColor, StringAlignment.Near, new PointF(4, (2 + TitleSize.Height)));
                break;
        }
        if (DrawSeperator)
        {
            int SeperatorY = ((DescriptionAlign == DescriptionAligns.NextTo) ? (3 + (int)TitleSize.Height) : (6 + (int)TitleSize.Height + (int)DescriptionSize.Height));
            e.Graphics.DrawLine(new Pen(SeperatorColor, SeperatorThickness), new Point(3, SeperatorY), new Point((Width - 3), SeperatorY));
        }
    }
}

class STButton : Control
{
    #region Properties
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Color ForeColor { get { return base.ForeColor; } set { base.ForeColor = value; } }

    public new virtual bool Enabled { get { return base.Enabled; } set { base.Enabled = value; Invalidate(); } }
    public override string Text { get { return base.Text; } set { base.Text = value; Invalidate(); } }
    private bool Pressed = false, Hovering = false;
    private StringAlignment _VerticalTextAlign = StringAlignment.Center, _HorizontalTextAlign = StringAlignment.Center;
    public StringAlignment VerticalTextAlign { get { return _VerticalTextAlign; } set { _VerticalTextAlign = value; Invalidate(); } }
    public StringAlignment HorizontalTextAlign { get { return _HorizontalTextAlign; } set { _HorizontalTextAlign = value; Invalidate(); } }
    private Color _BackColorTop, _BackColorBottom, _OutlineColor, _HoverTopColor, _HoverBottomColor, _PressedTopColor, _PressedBottomColor, _TextColor, _TextBackColor, _DisabledTopColor, _DisabledBottomColor;
    public virtual Color DisabledTopColor { get { return _DisabledTopColor; } set { _DisabledTopColor = value; Invalidate(); } }
    public virtual Color DisabledBottomColor { get { return _DisabledBottomColor; } set { _DisabledBottomColor = value; Invalidate(); } }
    public virtual Color TextColor { get { return _TextColor; } set { _TextColor = value; Invalidate(); } }
    public virtual Color TextBackColor { get { return _TextBackColor; } set { _TextBackColor = value; Invalidate(); } }
    public virtual Color PressedTopColor { get { return _PressedTopColor; } set { _PressedTopColor = value; Invalidate(); } }
    public virtual Color PressedBottomColor { get { return _PressedBottomColor; } set { _PressedBottomColor = value; Invalidate(); } }
    public virtual Color HoverColor { get { return _HoverTopColor; } set { _HoverTopColor = value; Invalidate(); } }
    public virtual Color HoverBackColor { get { return _HoverBottomColor; } set { _HoverBottomColor = value; Invalidate(); } }
    public virtual Color OutlineColor { get { return _OutlineColor; } set { _OutlineColor = value; Invalidate(); } }
    public virtual Color BackColorTop { get { return _BackColorTop; } set { _BackColorTop = value; Invalidate(); } }
    public virtual Color BackColorBottom { get { return _BackColorBottom; } set { _BackColorBottom = value; Invalidate(); } }
    private int _Radius = 3, _OutlineThickness = 1;
    public virtual int Radius { get { return _Radius; } set { _Radius = value; Invalidate(); } }
    public virtual int OutlineThickness { get { return _OutlineThickness; } set { _OutlineThickness = value; Invalidate(); } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override RightToLeft RightToLeft { get { return base.RightToLeft; } set { base.RightToLeft = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool UseWaitCursor { get { return base.UseWaitCursor; } set { base.UseWaitCursor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new ImeMode ImeMode { get { return base.ImeMode; } set { base.ImeMode = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool CausesValidation { get { return base.CausesValidation; } set { base.CausesValidation = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ContextMenuStrip ContextMenuStrip { get { return base.ContextMenuStrip; } set { base.ContextMenuStrip = value; } }
    #endregion

    public STButton()
    {
        BackColorTop = Color.FromArgb(60, 60, 60);
        BackColorBottom = Color.FromArgb(40, 40, 40);
        HoverColor = Color.FromArgb(80, 80, 80);
        HoverBackColor = Color.FromArgb(60, 60, 60);
        PressedTopColor = Color.FromArgb(70, 70, 70);
        PressedBottomColor = Color.FromArgb(50, 50, 50);
        OutlineColor = Color.FromArgb(30, 30, 30);
        TextColor = Color.White;
        TextBackColor = Color.Black;
        DisabledTopColor = Color.FromArgb(30, 30, 30);
        DisabledBottomColor = Color.FromArgb(40, 40, 40);
        MinimumSize = new Size(15, 15); Size = new Size(140, 20);
        Font = new Font("Verdana", 8, FontStyle.Regular);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent.BackColor);
        if (!Enabled) SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 0, Width, Height), Radius, new Pen(OutlineColor, OutlineThickness), SharpTwist.CreateGradient(Width, Height, DisabledTopColor, DisabledBottomColor));
        else
        {
            if (Pressed) SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 0, Width, Height), Radius, new Pen(OutlineColor, OutlineThickness), SharpTwist.CreateGradient(Width, Height, PressedTopColor, PressedBottomColor));
            else if (Hovering) SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 0, Width, Height), Radius, new Pen(OutlineColor, OutlineThickness), SharpTwist.CreateGradient(Width, Height, HoverColor, HoverBackColor));
            else SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 0, Width, Height), Radius, new Pen(OutlineColor, OutlineThickness), SharpTwist.CreateGradient(Width, Height, BackColorTop, BackColorBottom));
        }
        StringFormat SF = new StringFormat();
        SF.Alignment = HorizontalTextAlign;
        SF.LineAlignment = VerticalTextAlign;
        e.Graphics.DrawString(Text, Font, new SolidBrush(TextBackColor), new PointF(((Width / 2) + 1), ((Height / 2) + 1)), SF);
        e.Graphics.DrawString(Text, Font, new SolidBrush(TextColor), new PointF((Width / 2), (Height / 2)), SF);
    }

    protected override void OnMouseEnter(EventArgs e) { Hovering = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { Hovering = false; Pressed = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnMouseDown(MouseEventArgs e) { Pressed = true; Invalidate(); base.OnMouseDown(e); }
    protected override void OnMouseUp(MouseEventArgs e) { Pressed = false; Invalidate(); base.OnMouseUp(e); }
}

class STSwitch : Control
{
    #region Properties
    private bool _Switched;
    public bool Switched { get { return _Switched; } set { _Switched = value; Invalidate(); } }
    private Color _BackColorTop, _BackColorBottom, _OutlineColor, _TextColor, _TextBackColor, _DisabledTopColor, _DisabledBottomColor, _ActivatedColor, _ActivatedBackColor, _DeactivatedColor, _DeactivatedBackColor;
    public virtual Color ActivatedColor { get { return _ActivatedColor; } set { _ActivatedColor = value; Invalidate(); } }
    public virtual Color ActivatedBackColor { get { return _ActivatedBackColor; } set { _ActivatedBackColor = value; Invalidate(); } }
    public virtual Color DeactivatedColor { get { return _DeactivatedColor; } set { _DeactivatedColor = value; Invalidate(); } }
    public virtual Color DeactivatedBackColor { get { return _DeactivatedBackColor; } set { _DeactivatedBackColor = value; Invalidate(); } }
    public virtual Color DisabledTopColor { get { return _DisabledTopColor; } set { _DisabledTopColor = value; Invalidate(); } }
    public virtual Color DisabledBottomColor { get { return _DisabledBottomColor; } set { _DisabledBottomColor = value; Invalidate(); } }
    public virtual Color TextColor { get { return _TextColor; } set { _TextColor = value; Invalidate(); } }
    public virtual Color TextBackColor { get { return _TextBackColor; } set { _TextBackColor = value; Invalidate(); } }
    public virtual Color OutlineColor { get { return _OutlineColor; } set { _OutlineColor = value; Invalidate(); } }
    public virtual Color BackColorTop { get { return _BackColorTop; } set { _BackColorTop = value; Invalidate(); } }
    public virtual Color BackColorBottom { get { return _BackColorBottom; } set { _BackColorBottom = value; Invalidate(); } }
    private int _Radius = 3, _OutlineThickness = 1;
    public virtual int Radius { get { return _Radius; } set { _Radius = value; Invalidate(); } }
    public virtual int OutlineThickness { get { return _OutlineThickness; } set { _OutlineThickness = value; Invalidate(); } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override RightToLeft RightToLeft { get { return base.RightToLeft; } set { base.RightToLeft = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool UseWaitCursor { get { return base.UseWaitCursor; } set { base.UseWaitCursor = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new int TabIndex { get { return base.TabIndex; } set { base.TabIndex = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool TabStop { get { return base.TabStop; } set { base.TabStop = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MinimumSize { get { return base.MinimumSize; } set { base.MinimumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MaximumSize { get { return base.MaximumSize; } set { base.MaximumSize = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new ImeMode ImeMode { get { return base.ImeMode; } set { base.ImeMode = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override DockStyle Dock { get { return base.Dock; } set { base.Dock = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool CausesValidation { get { return base.CausesValidation; } set { base.CausesValidation = value; } }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ContextMenuStrip ContextMenuStrip { get { return base.ContextMenuStrip; } set { base.ContextMenuStrip = value; } }
    #endregion

    public STSwitch()
    {
        Cursor = Cursors.Hand;
        Font = new Font("Verdana", 6, FontStyle.Regular);
        OutlineColor = Color.FromArgb(30, 30, 30);
        BackColorTop = Color.FromArgb(60, 60, 60);
        BackColorBottom = Color.FromArgb(40, 40, 40);
        ActivatedColor = Color.LimeGreen;
        ActivatedBackColor = Color.DarkGreen;
        DeactivatedColor = Color.Red;
        DeactivatedBackColor = Color.DarkRed;
        TextColor = Color.White;
        TextBackColor = Color.Black;
        MinimumSize = new Size(40, 15);
        Size = MinimumSize;
        MaximumSize = Size;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent.BackColor);
        SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 2, Width, (Height - 4)), Radius, new Pen(OutlineColor, OutlineThickness), SharpTwist.CreateGradient(Width, Height, BackColorTop, BackColorBottom));
        if (Enabled)
        {
            if (Switched)
            {
                SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle((Width / 2), 0, (Width / 2), Height), Radius, new Pen(OutlineColor, OutlineThickness), SharpTwist.CreateGradient(Width, Height, ActivatedColor, ActivatedBackColor));
                SharpTwist.DrawString(e.Graphics, Font, "On", TextColor, StringAlignment.Near, new PointF(2, 3));
            }
            else
            {
                SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 0, (Width / 2), Height), Radius, new Pen(OutlineColor, OutlineThickness), SharpTwist.CreateGradient(Width, Height, DeactivatedColor, DeactivatedBackColor));
                SharpTwist.DrawString(e.Graphics, Font, "Off", TextColor, StringAlignment.Near, new PointF(((Width / 2) + 2), 3));
            }
        }
    }

    protected override void OnMouseClick(MouseEventArgs e) { if (e.Button == MouseButtons.Left) Switched = !Switched; base.OnMouseClick(e); }
}
public enum MState : byte
{
    None = 0,
    Over = 1,
    Down = 2,
}

public enum CState : byte
{
    Horizontal = 0,
    Vertical = 1
}

public enum CColor : byte
{
    Black = 0,
    Blue = 1,
    Red = 2
}

public enum CStyle : byte
{
    Round = 0,
    Square = 1
}

[DesignerCategory("Code")]
public static class DrawX
{
    #region Grain Texture

    public static Bitmap GrainCode =
        (Bitmap)(System.Drawing.Image.FromStream(new MemoryStream(Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAIAAAD/gAIDAAAAA3NCSVQICAjb4U/gAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxEAAAsRAX9kX5EAAAAcdEVYdFNvZnR3YXJlAEFk" +
            "b2JlIEZpcmV3b3JrcyBDUzQGstOgAAAAGHRFWHRDcmVhdGlvbiBUaW1lADAyLTEwLTIwMTDgo5jkAAAfbElEQVR4XmXcC7JkZa6D0TMMKKAOMP9B9vL+MkVGtSOuW5Zl2f+uB8Uj7tfff//948ePnz9/Av/+" +
            "++9ff/0FfH9///777/g///wT/u2332DMH0+k0YonQCqN0yi5wQDZP//8Q49Hlills2FKmko+Mj0GRhqpJTO0roOVDQq8SEMvukqZ0hYyGNhsGjy92R2fBraI8qy/v3Xd8+X/hF5PogC05a7JC49MDGcNIzNt" +
            "ih5pmdh4x+Wsu0GaniEjWwokw3cDXmaCkYkjmxLtIgYIzO7agMEO6E7AFKbQTeOZHDaV2JY5fEF6fY5OMYA0A3QuaTgBZWc1uHNNiRaIfriErsBQIoWSP4ZzeyOz6no+cgylTGOqQbxBmUyLQ0casTFMoEvf" +
            "ik4yAneYFpxVr4PNDtO0+h72/f2FFVyw6nSyGVIDyOfCl28ruetykR+f0+QOmO09RgBu+J6qRGZuChCYBL0f0GoXYIvBQCvoBbKpWjAGnzkcaXu2TQm2GKGVRjBRis7Ay6wMskr85f9Q5hPJd/77R0nAu170" +
            "Wu4tzpem4zA0SAHg+YdzmA+xVp9DadyWPJl3dIyuPEzf9t6QmJtW6yhbSinabpEubKqPxUG3nAOy19GHjdSVkfczC2WBNoqCXTch++RKoZUjgansupUmPitiAq0ctBoPp8QAGEqyz71CKVtX2TsFbF0+pgg+" +
            "31PU4jYTXTKl0MpHjm8LT9vh3VAZT0b8pTbvYu0wQNG+HDEt2zdqJCCyM1UoZVMcsiWGaeZDYzDnnocRsNk+nMFWdJISD9AEYjg0S0mvRDq7y7Oq1TNlrR4iYwBWZLDoJLJ2ERtHftWgbtIMRW8DTDZjgEBp" +
            "TCBZI2VdJgbxcjJMuwGCrtdKoxRADoJMF2nEGa3uNru6TcakMZIhXskKyMoKWFB2m0DCSJiGfiQfDsS6AUzjBH0KDHAfS9EXEVssKHQdBPSwrOkFI7muABqXuWGMOCufVnITSpp40Qpds4Jyb6BR8pHJWkrM" +
            "thUw3tSd+/7FRaZFUMw8Jec+KNsyQaduBZBb54msvkg1qDUYzVcPzgvIEajFmhjGKLVgg7DM0Mpei++T0TPJDamkFLpI4tZ1tHFlD1PqppHzFHZh1mWVpldkG05PwBapBWux2vYpteSO0doI8f2eBXVZCrkB" +
            "OoARHZKszyTwMpIRpdm+l8BMhuwsJMzKSAETWCFM9QlgpFYfvZNkXWS3Adw6QEv0GF05XuamJbe0UiQwgmfbIli2nTLbDPvxSAbcxwo1Y0BuXzh3gEUrDWO6QC7wfLREfFuVLpPn0H2y6CakrGtjeuJ28SGT" +
            "e4ncPVo0XYhvo0EBx4tmkfzLusIWmFWXdEbkANmeEPPVEYy2rEZZ8JJNtrjsDkqbAMrnsNevEUETZt4dlEqLtRppVgsmbpEt7TIli8TA3g/TKAGXyDNXGmxEAJRIgsaRgHUCmb+8Tw/LRmRB/7ni/naH1A63" +
            "ss4Ik6KjZZNKAq0mySyQlX0dLaAdfRSbgPalh3OAubXXCFLOJE/AFFn+mXA2IpDJ8gHKBBhW3V8Yz1ZX6NKk1C1nzkS3UljawenvT/CK83hCD9OyzrVJaMm6jGzFk8U7XdkFgBIWlHAjWrBTCJS6Zvkj4zGU" +
            "MiXP7stNKLXkzpMb0eKGN6KUdYHOcydBi3Y8oGVcKwddfI/SxcDEzQLKpu6vhqQa9WBGwHiOGHcIk3LirtEFKLUAXRggsFvJqof1VGQjsiCQGweIIxtvtq5dSLhPo9v2O+U5Rm6WWGRV7kJWgEFdQAvZFlYY" +
            "PLHcLqGLV1LK9zMrl5Y5RRaYSiCSOl/B0RTeFJIG06EW4FsMIG0iwyxz0NVqC0Gv4ll0XEuVMoGAjfRg4/gELSJQ9glaRNwKJUBJRkyDkTFk6c1y0J0MY0qXA/D6o4NCpMBoG6sEmDYjkE32npQyzCf9I7xI" +
            "I+gTtEhgiAEyfLdmi3RDGutgLWJKJcGuUgK9jT6Tjc9ZF8jNbLJI47vqLn7fDNBrkRXI+w2eLvdcBF1j1AmorRGUggbTMlPEsLzLCMgAYVzJBwN3tJheIAmU9HArdgClsB05xlSGQOviZWWGfdNkaQjCXc7z" +
            "Tnz/PkVcaVyml/dB7x8rE6mFXjmp+V9O0VUGTOnKugunaJVbuRci5c5NEOaQlZKSuZHIpjB9vm2Xa2FqwabCunJTnQTwJ8AbhON3WOZplBxs75JIXfz9nqWXHWAe1hMGMEC8eJ5wYQfSjsa3uNnWAAlg0ZWf" +
            "tgCmvUYIMKwAXYNIXSQfDq0gyN+UloAbF1stCyMYJbEuN+Ot2DiBAGy0iKazae7Nz1Ji8d9v8LJGRoYBO5JGsmPkAmKTgaZE3ZRa4ca1YAwsF5jGw3bFG0HmL+Md07N7c54Y0eXGO8MiQWOkXUiy/NtFoEsj" +
            "aGQtAhFPDxisRdNGgtfvWep5CQAfaBgwnMakgM27FTaOZy3iE3PQksny0aJE9gazlYAyQXpWAJO+KcwzWU8yWGCEFl4Q6wIc8AmYcKARtuRPVhC0Tqu3hOVKeoZfhtXZ9VTD2niAGk+gFansMox5AiMBPGBK" +
            "iyxA2d2Z9HhioXRQtzKHCyNkPSAfXVmrQYa6lbAuoKRXmlXSA6zq2q5VmXMmAugSGoKsjE8cye1+GULVGtrYPt9IkwBSZt0d8jbJBgECvDLQ+q4nSNNg98FaDZqij+l0t+UgzObcrBIuaKwQWkIL00YlE+Pt" +
            "ygHuDLwpArIWKRvHVPJp3OD9M3hFlPl2CGUu2TUvaEzqGhTdisfAAIcBAuBTYzyAIYAFGd4iJGCdA5R4oJOQGOJuy5meedhVNL0KI1jJxgFZt5P6WGQMd7aM77HIlsbYAhO8fs/aYjrRxchMTTaPpwfkxJ0+" +
            "MTxDsmWkbt8iGdIdyjRGmAi2bSm6BAkD3dBXq8QrxwOCvl0wWbtgZLwDALK+glYv7TACuGxQMCG4jzVHoCNqU8tMOWoheQGZVurKBLwSmwojaeTO4izjM7kv8XwLOT0SoIcFLDjIRjDEvYQyJhlSKWDMnCM5" +
            "FLq1gJR83KZFppxG2S6MnBh+/aE0azWMcXFnwY/zqQnwvGhkYlELI5R7qkHRPi03hXnStK4Rrc1yO6P3bwVaPcNs27MV9KLBfjA6CQMYkZEG54MRVnRJe10C4wnunc9zBFJ+rrifKEaU9986kKJ6Rsele666" +
            "T6mkBuZopMtMTV9JoxUPGIRdtvUESJl5GCDQDdO3LpNK46JTtTI0jkkjKrUELBM3m62Yg0ygBXhR48QZYmBAEMsE9xu8/5HVHElluhmRcqmbWGC05t4z8BsUBunb1HGRBMQBpC7AyjiBEaUsMlcCusQwPVIm" +
            "mBsBJn3rMulmwVlX8FF2DDFMvxGRiQy7CtZq+/0y7OUo8yIvZCA7kzm2JhdkXWRfilv3yQRCN1uztZAwIBthBSC7jz5BtkJJnIMtW6SFwStFGl36sohhWylg4ngjvR3JUMZ3GD6B7BJdcb8Msysnqs2607lz" +
            "ZKQrACWSkkwJUxoXDPfCZFo0MkYkEEC4WVYAhgbAB1raOwVSVt75zyfrnk3ZSA84D24LQEaTW2I50Ei5U2ERyNPG+5llQEGUzoBrAGUtOvtaD+PDSOP0ciP0XUZQ4DPsebsDkz893BkzzLNMLNOkzLY7eToD" +
            "yJ+DIO42QQkv16Jseye1vZGsmOhiRFN4zP3bnTahRJOiZUJLOC67vlR6GRa5c5wMRsppPBjGA92Bl62LSSmLbiDu1okpy0gygvbqzqGlbUTywWQl38Oe7yIrKclgZ1vaavrE3NL0Awzc3xti1WYabqYrqUUt" +
            "DBkxgHEKIJIBNAAek4/FwghGNiWLlNx0KTHJmt3HQpaRbWxRgadEcmNSmRvnDjZY3NbnAKTuPgrGCIYPEkOAUSaW539/KC2026RtxlkiaSdSw7mn6T4jnUUGA5FKynjKjlB2QeOyvRgjvRAJFEyM9Iy2IOVO" +
            "QgIYmabV6csjrTOlpBS6wqKOxOt2jNglaQhgPPJ+g6+3J4luMkbUWF0ACVgvegBGKGWMrhF8t+LnjFHq3vADLKKRtWhkYktl+gBb2GpdGIhP30bBBCnDHUxGn3M4E3u7vGe6JH3XPmbnTE8gzLb0/oUFViRF" +
            "9QZt5L3p8RLsBNCapoDcTSmNFwnotTjb0uMBLZmtTJCeTEbuDZ9lPmVT8saZWyQH6PsWWgSuhROn73XwnJ/R+wnIVhAETJUxMvHrX99HWabdyoJFO+QEKbXcAQsa2KBMIBPQd0E+yeyT8br0Mj0xRilgAqTo" +
            "KgDfCJ+6QAJkzq2DAUFgV1+KjDjcdob5xNBjlEYcIBIATQmGZF9YdiL3Lp61stYnEJ2FoRH0MB7oMXBrjGB4EgD4jSg3SEAJ31uf3+atQMK9PF4Y0e36vhQZgBf4ugIPt5SYOVmRfxfCBpXpO0OGDS4o72cW" +
            "3XPGBUrGUMtEPVVm0RFpelgnwmWxL5JDQO4s3ayajccoYeYyPgctwHZZbIptzimLbPFIG1taqxECDI2WRZ0aYwXNTjKCsV3refH92Iv7q6H/4SVTC8AAPhGgzCLH5lvGtLPItDBkGGWMMAV3rq4wDrcUZgK3" +
            "ogdg5GfP/QwKMOlVQG/ubcIUQbNIMthgr4VFxxMYV+LD+VhdptEFhBYsMxSvP5QaU+QeMAl3Vo40ovW9jSa+3WQWyLqVubUiEyBzWSDX5dN9xrXk9HgmADeYcib5U8qtJqiFSYApW0RjtttytoKSoNUtAjC9" +
            "Ih9Kcf9hiP/JAtCTW4xpDNj6BL2EpstkMi0AKZAiDZCJwdYvWiTDHSc7RsYDoimRDyaxqTkjASNtJHCJMFsLT9yuWphsBf2sZPgWv39dG8Er789Z1YIXbDL3Lo5ZaQawjGNHY/Ar0yg7lC1czNZuIFlTZM3i" +
            "mStp+kyZRGLoybYLA8h1MUojTWGsQM6Hf1OOiRFwAmIZvs/5/ivsZPdXQ//T6YLCHdTG4B6DrxTEWRixA9OgEQyevpE+TRo8gVmlgJkA+N3nkt4gG5R15awI2LIqYwKUbOOJycKmgD6TaEWlnINsOwcME+NA" +
            "JmMSZH7/abcwqQ10H6zdQDhSUIrsmuoTEKSR+RhBcpOVNFZ2XLnrcwZk4tyQfcRmKTHCoO5ekjlbGdPlugkCyfoc2Yq6yMru1G2Rbh8aLzq47a+/Gk4hW0nUmwteBI/zWWsJU7nEK7XIMMIU3CzeccgeZlbW" +
            "QtJE3svef4WmdEYtgZGVRZ5MZp5VmsRIActp6PFsy0jiNMJD5AkcQJAnngwQ94+VjUFWtj5RCtm83Evk+xLvaJlsmcG+tbIu7EQgXvDnicHPEJMtARMt9wAYghw6MoHMsKUGAUxLMX0vmC2xjRzwkYIAs0Vy" +
            "hh32fIPX761tB0T+r3+snMJupaAzj2QE18VQmoetIQ4LMkwLyPCU7TDLEIOnwdNoYTbV0j5ZtniDlKKSILc5YOTWdcNImSx/+U58fnI12CWiF2Hg3mi7gE3tAG6C+P55VkZqM0TUKWQYQ8cxrzImUsYA7egl" +
            "3ODnnvs6yXoSvl2AjJExLSVrdetkmEOGZQItSi1ZkKXXJd43Qiphg71C5GadbCS3bEUHj5TJ+DR+vwybsZu045TsJlLKtfD0cgfJusD07ZBhm3LWggOZVzZLz79XwRu3jkZLaRAgIAMaSSZTEsiN8C8LPHFT" +
            "xDbC+G7TNU6glW0tPrr5Y7Tk+6ODYC2rteWsZWXuXATAXUYWMDtZcKBnRSPrNmIxz/jt1qXXbZ0cQ3Ze79/IjRDjDSrhnkEPKK3Aw0aAbYQrRY9qXfcrkf8ffbLlxq3T4nl/u9NBikXX5A4bAxo25gj6zhV4" +
            "QYbvUMGTkgw/cVZwetlIW7JF5oAUBB2dc4Zys2Qw0PthjNIgQYZ4I6LtjeAxzoAn6KOwEoCSVUEWf390MKxugTEMBfUwAeuwffNtvawFuJISEIDA84yhNyhYaSnpCeBWzFbg5ZaaxdO4x8jyLXhPCRifv2hc" +
            "NugGXT57oLIn695N7z9hGEESyMpehJSR9zML1bkUHWFHXkotugRA65uXBdMJxNzMivZlJYhh8XzAi2foxDIxQ25we0VijEyG/8y6PdJhQonEuF/uWliwSmZQ6LY3c4MraXIQsKuyff3MqtajhpEwnqLcGCzg" +
            "noQxJWO6WCaOYaVLBsjCHYvu5tZZOecQMAWYkp2UuNuEMoFMzAEJYBxAxsd2iwDiXifrdlWzGSJhGe4YU80S787XP6JpTK1MpJ1O1kUCMjtdI30OQNm4rsDXbbBNyswBGZlJtxKUBRJOABBsBXMAkz5G3iU2" +
            "9hxdAdMjs4Ux4zFtYYIE+BRdDuD7LJncH0rNmNQGUoSzFkiOGLzAy1zCTfFpBz1xp+BlTCVAgKScOEYX04fIP2eg1XJTXR+5QWW2FqU0nozelgIv82xRYUS0lEl78ekpMbD8+k+7cxHcsYZhy8zMhVIgmzeS" +
            "F9IUjanuwHe9UHa0AGIEZ4M5tEsoYSbEulqyXWFAtiUZh6bSILuhFbrK3qxEto4zsVASpCEga0tBsBsSk93PLDoRyJECsMBuPEZ0RzxSeR/g/SPcYr5ajUwvMF6ly7ktygDZ8+SLpu6a5578TeUvE+BpKpOt" +
            "zLy9PbJZGR/Iny1BymlYNZjGbZt1m+79nmXSstYkVSIBXdFLMAaaFO7WwhgRmF1jHFNXKbQMtiUHbkhgJmTIfAR/5/b5DBZZiTN9cCsM8iHmAw/omsqkRV1ukKBuJIbebZSybneacobQff0GD+WObYAjFySA" +
            "NCYAJSC3bEcIJId7yhNws5QxidNHApXhhRtyMy6yorRalxipZOi1yLIyce+kxLRXngmx7TSYZI0gKYs+QkcCKe/fSKMUo9LlwloZaLfcoQLQTd8+Uw6KpGSOp7Sikfj8934H9Zgu68FG0ojGgQSJAYHX5Ymh" +
            "T4ZJ5oCm8F2iJK7UBXRFKwACrUyyxfeW+6cOFGqbUD0GKVNrdbpMnalwSl+q0niLYYNtwhAgxae+T8AT2SmUSK14pVYH5LbtxMieJDp1DkphC72IJ8vzF72SRinwPBuUWRGQ4TFtRN7Hmi+253XNXqV0PR4D" +
            "59hignbgBZPc6bWA3jB+/oKADNkNNKLPJGOE8e3tGPoGs5K7EN/bmAB1tRLQN6JLw6GrTD17Xl/NiJaSWKaxGt/lr399b5lMlx2A73QtpDCJN9kmXqzzJb5bnl9cATIj7aPH92YlnuenbRuTic+9uoASlrU6" +
            "gBuMlEVvptQ10q62l3PQ7WYYEEYo81FyJibDC1NIrcD97c4NPe4yNUaQtritsDBDIMNAPI1bzZpSykZ0n1fcBWRyLbZKAibyc9IrmBDo9nJhJNsGjWjx1MLbiwfwImxvuzCAFWSRHMxySNyFSG4Jular5zBJ" +
            "TyCL+/eGhmVhphMNNIkRMYABwALYlNwpYj6J211JT8Y5T5kSo6tVwAIpCDoUz4pY2eo2AroAQZ5m5RhKu0zd+55fs/FMYKCuwHde9wNIuQNgoJzy/klpqAFAYLrAso6AK+VG8I4GCjuE2awLSiOtVLrSlNKg" +
            "oMcQwJSw3AFIpV2m2qgkk413AM2CrWC4nEw2Hjar5ZJn+W3sRcQ29pnwSi0BiD6W0vjr/yWU3B3CHXIzhvG84O6TlchtBTjIulnrYigbgckYAnW5tRFDIGBBRqDVlloG4RyUukoBiPRmu1nsnZGUtggOjQh3" +
            "ypQMtZRwVsoeSy/iyeT7ZUjB0cDa1rSDCMYI67s1RxlJ9nyTexsHGoBJGnplDsnwGDyGIMZgGuNKZ8GCbBqhJFBq0QAG2wIIzj04BwJRSUZAqYwXu7bPSiDTlPEAPStKmvsbaaxJYxyVzbQYT4o0IxMgTSL7" +
            "IoIeowX3mAJPQ4nX7SY8oMVfSf94nEDoyg5tl1Irc1OJkQEmdTOE3Waws5mkTGxdpXsIur9B/KxgysR1ibMiuI+lpwGwqOE4OrEujDdZmZ3AE5MhA0xs5SMw4tMcbhBPxkewhWl6FU1iJNwzLEoGyL0WlpsV" +
            "bFvaVbKgxG8L3Cw9MaxlBT2QFaC1vbN6/fMsRsa0UcragFyLe15txbSJMkfRHWnolQx7c7OYZsNyMl27YmSeghIWWvmkaZFMLJCUsuAszLaCucFCF2NK17gWYC+sS18rn8f49eNBo0Uj33+fpa3oPUq5GdiJ" +
            "SZUEaRyHhNct0uPtoOnBZS2RCYBM6VahNE6WuRLo6yh7mOjxshHdvks3r4tsC1DkqYs3SEOcua5Wmnx0AeJukykzx99v8BV6xu5NzwwX0dY2PYb3c4TAiIzsekGmhYGB7s4ZEPRlJB85PqVdHUffd8TQIDE0" +
            "gQLfUynxGeIbx+hqdb/YxjSilmMwtawQ3d+6ZPlgiO/3rJABbOoehikr8QnaAffhANmIFh7ZSqA1GSozkQnYBozvczTOTRAk49AX0WqLETwrmVIZTxYwKAK50dBX0jTOEI80iAe0YKDjlXA3Y8Trv/yDDMh2" +
            "ZNePjy5p7gSwQH4eJ+uKcIuNzAroSoMDWdlIjMzfUhHAy3iyDjC1Wa1ImFKwwiiNC4wLCYwIXaFrPFtivJw+bEoWZDKZlilA6/U30q0HGqunhWxB7n0gGA9jdB1hSsaLmDyVvpRI05ZO73llgZcJMKaImWN6" +
            "AzcCPloEZLl1kq6YVTKDXYvJIU3jNBwIYBqRYA9skUhTef+IJgUqu3qOq7RMti9G3Oufn1m1AK3H//UXJhpkPLfO7QgCjGxQaBHLuj1JNzea/MVtffZiZEoCvKkGvQJol/EeBQsyPE0MINPwMcJnGwnCRsjq" +
            "1mrk/qmDsUQ9z6ZEAuhKrU7ssnYDNIzwzVLStzVboexou8o5M1HCjcgxsG6exAIg0xLdw9b2DiBAYrpEF4/E4I2wbUsAL7eREtmgLPBGAsmyIrs/Z1V3RPNyXrUEGTvATIIAhpHoJvqNa83NbuNNrdt9Bu1V" +
            "RnYlGSwawSQz0tJkZRrnyXUp5T4ZrEUm8xHdIzqblRZxU0qGn8/ZLiP3LyywBuaCKYt4AsMs8tV1lnlZq6cS4InTE+M5lAWS4POaxB0kaBimV9JkZSmNHA7oEmfYN8LgK4WSkpVW5R3xnNFsXXrmjhFWy8p8" +
            "BKUVQgt/fyhNp8BmLZSCHZKvSZgYSd++ZlmHIzlQJjMFnN0jgLkBNHfCE8Tp+0zpP6eQ6ZU0PGEMjRayt2VlaVMpOzUlWaRA0pfpMYDspZ0h03eVXQ3ex8Jq02Xa2Ocn6IsYoIErEzMi0GpKq8W5fT4PMBXf" +
            "TcQAElCm71W34/1DjcGLbsBsSsYzLxwjgJRh0Ssoc7NId28kQGJotPBduBXZ6t4fSoVeV5ohzbrA6KaOly0gxvchRHcgO6VWboIA00E0Wh3dHRg8gYzvRFjLSJqsEsBtx7ASBJj5dxJB9xvp7Jm0q+gSwcSU" +
            "Eknf0k6VCe5jpZCJutK5zw2naBN3pGw+U8ou6DMh973wZlsm2o3RVQLGdWP6NBYhm5LpC7Pt6gBdSiZ9CDwHmKYuUjfQXtlIl9NTGmlW/HIV8W6AAUGg5H9/dFDnlW+9xpQNhGULlG0y0lSCns0KfoZujYwx" +
            "Kwjoa1Gmd2KnKw0qKTGCjDnAgUAYh0VfB6OLBDq+swmQtpPVhbUEZrNISjlnVgNW0+jKypjXvzesBzCyUgmwhntAfOvJOu48Hkwja9lkVkxMZqUuRigt6j2CoF0TCIzSOHOlrqxkXldoZdVGXW49AUDGT190" +
            "J5JGCWPuDe/PJHSb6vLE8ffnLDrq6hp0cBcrdQ07RXDHbBPQq4DGva2si+kBNALZC80mgLOqRYMxLmCDeALRijTtpYlJDJulVzIUgfZ2TBeKXYUxSFAXyVAWOejClF59/8IiIw06od0CIq34BJ1uBNM8R5qu" +
            "CWAM0ugaEWaTAabMElASCKArAa2YMDEfs+GCT5cMCDyNcF683GsZMiEg6+UAsll8TwPoCQRGiwOsJfcp/vv3huapcwlrtwnmlW/zRmTBBU/WSj4CwOCJKWma6giGsC5/mQCfT7Zm96QuDgjd7nYSEE8ptwWT" +
            "YVYYSoKOgfMXSLJ+UDuJCVkkJb1g2y6G9xu8sSiYrq1m8sV0t0wzJlMZbqpBmSYHuJFttRJ2EywCK7vEXnjKGKXoJDIt2IodoOt+QQAjO1Ug6eV45lpsMXD+MqVMgAe0AAeYgoHXx1KbR9ltxnAXYIBupe7N" +
            "WaRJ1m7iZpvqlE7vSUIp+gQC3q5uMkUstzHDrITSDRka30n4RgQs2tidSDK7KHdJI3I+yWBbupAykJ74/t6QLq90DQM7iE6GkcSwlugZgEGgfbrl9LubwBZl75FNkcltAeZQmNISZnVTcjNOeRe8f7lRAukN" +
            "YgDbbewwI7r0+MpWdH9fXInHDDwb7pm6xu9nliNQ2h2EybrvJTb2yw5iLqKbwvlghDVyXydDTGQ+PcAsJoEcbkXfVEncd4xkyAfG0ANmxT3g+egEAt+sYBhpHMYDIg2ej/AE40yMyLpN4e/vDSk02g3nTgcg" +
            "9xJBgDc2XNkgRyXD3ACl8Ud10fu1CIAGWSmTeS2mZwOdwRYgizc1paW2xHOAuw1Qwl1CWXfm8cblRnYb8w7TpawlI++PDig1ncwl6yyyixFZ22qqmwBdU7pbKaxsQRGJaZfcgw1yKJQ0urkRyFZ0febJIgGR" +
            "XlmuRcaq1wqlLhPifCprZU7cwQZ1G8Rww7D9+fPn/wDyxUe7iVXM3wAAAABJRU5ErkJggg=="))));

    #endregion

    public static TextureBrush Grain = new TextureBrush(GrainCode, WrapMode.Tile);

    public static Point[] Triangle(Point Location, Size Size)
    {
        return new Point[4]
        {
            Location, new Point(Location.X + Size.Width, Location.Y),
            new Point(Location.X + Size.Width/2, Location.Y + Size.Height), Location
        };
    }

    public static GraphicsPath Round(Rectangle R, int Curve)
    {
        GraphicsPath P;
        if (Curve <= 0)
        {
            P = new GraphicsPath();
            P.AddRectangle(R);
            return P;
        }

        int ARW = Curve * 2;
        P = new GraphicsPath();
        P.AddArc(new Rectangle(R.X, R.Y, ARW, ARW), -180, 90);
        P.AddArc(new Rectangle(R.Width - ARW + R.X, R.Y, ARW, ARW), -90, 90);
        P.AddArc(new Rectangle(R.Width - ARW + R.X, R.Height - ARW + R.Y, ARW, ARW), 0, 90);
        P.AddArc(new Rectangle(R.X, R.Height - ARW + R.Y, ARW, ARW), 90, 90);
        P.AddLine(new Point(R.X, R.Height - ARW + R.Y), new Point(R.X, Curve + R.Y));
        return P;
    }

    public static Size Measure(Control C, Font Font, string Text)
    {
        return Graphics.FromImage(new Bitmap(C.Width, C.Height)).MeasureString(Text, Font, C.Width).ToSize();
    }

    public static void Pixel(Graphics G, Color C, int X, int Y)
    {
        G.FillRectangle(new SolidBrush(C), X, Y, 1, 1);
    }

    public static void Gradient(Graphics G, Color C1, Color C2, int X, int Y, int Width, int Height, float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        G.FillRectangle(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Gradient(Graphics G, Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        G.FillRectangle(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static LinearGradientBrush Gradient(Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        return new LinearGradientBrush(R, C1, C2, Angle);
    }

    public static LinearGradientBrush Gradient(Color C1, Color C2, int X, int Y, int Width, int Height,
        float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        return new LinearGradientBrush(R, C1, C2, Angle);
    }

    public static void Radial(Graphics G, Color C1, Color C2, int X, int Y, int Width, int Height, float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        G.FillEllipse(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Radial(Graphics G, Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        G.FillEllipse(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Image(Graphics G, Image I, int X, int Y)
    {
        if (I == null) return;
        G.DrawImage(I, X, Y, I.Width, I.Height);
    }

    public static void Image(Graphics G, Image I, Point Location)
    {
        if (I == null) return;
        G.DrawImage(I, Location.X, Location.Y, I.Width, I.Height);
    }

    public static void Text(Graphics G, Brush B, int X, int Y, Font Font, string Text)
    {
        if (Text.Length == 0) return;
        G.DrawString(Text, Font, B, X, Y);
    }

    public static void Text(Graphics G, Brush B, Point P, Font Font, string Text)
    {
        if (Text.Length == 0) return;
        G.DrawString(Text, Font, B, P);
    }

    public static void Borders(Graphics G, Pen P, int X, int Y, int W, int H, int Offset = 0)
    {
        G.DrawRectangle(P, X + Offset, Y + Offset, (W - (Offset * 2)) - 1, (H - (Offset * 2) - 1));
    }

    public static void Borders(Graphics G, Pen P, Rectangle R, int Offset = 0)
    {
        G.DrawRectangle(P, R.X + Offset, R.Y + Offset, (R.Width - (Offset * 2)) - 1, (R.Height - (Offset * 2) - 1));
    }
}

#endregion

internal static class ConversionFunctions
{
    public static Brush ToBrush(int A, int R, int G, int B)
    {
        return new SolidBrush(Color.FromArgb(A, R, G, B));
    }

    public static Brush ToBrush(int R, int G, int B)
    {
        return new SolidBrush(Color.FromArgb(R, G, B));
    }

    public static Brush ToBrush(int A, Color C)
    {
        return new SolidBrush(Color.FromArgb(A, C));
    }

    public static Brush ToBrush(Pen Pen)
    {
        return new SolidBrush(Pen.Color);
    }

    public static Brush ToBrush(Color Color)
    {
        return new SolidBrush(Color);
    }

    public static Pen ToPen(int A, int R, int G, int B)
    {
        return new Pen(new SolidBrush(Color.FromArgb(A, R, G, B)));
    }

    public static Pen ToPen(int R, int G, int B)
    {
        return new Pen(new SolidBrush(Color.FromArgb(R, G, B)));
    }

    public static Pen ToPen(int A, Color C)
    {
        return new Pen(new SolidBrush(Color.FromArgb(A, C)));
    }

    public static Pen ToPen(SolidBrush Brush)
    {
        return new Pen(Brush);
    }

    public static Pen ToPen(Color Color)
    {
        return new Pen(new SolidBrush(Color));
    }
}

internal abstract class Theme : ContainerControl
{
    #region " Initialization "

    protected Graphics G;

    private bool ParentIsForm;

    public Theme()
    {
        SetStyle((ControlStyles)139270, true);
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            Invalidate();
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        Dock = DockStyle.Fill;
        ParentIsForm = Parent is Form;
        if (ParentIsForm)
        {
            if (!(_TransparencyKey == Color.Empty))
                ParentForm.TransparencyKey = _TransparencyKey;
            ParentForm.FormBorderStyle = FormBorderStyle.None;
        }
        base.OnHandleCreated(e);
    }

    #endregion

    #region " Sizing and Movement "

    private Pointer Current;
    private bool F1;
    private bool F2;
    private bool F3;
    private bool F4;
    private IntPtr Flag;
    protected Rectangle Header;
    private Point PTC;

    private Pointer Pending;
    private int _MoveHeight = 24;
    private bool _Resizable = true;

    public bool Resizable
    {
        get { return _Resizable; }
        set { _Resizable = value; }
    }

    public int MoveHeight
    {
        get { return _MoveHeight; }
        set
        {
            _MoveHeight = value;
            Header = new Rectangle(7, 7, Width - 14, _MoveHeight - 7);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (!(e.Button == MouseButtons.Left))
            return;
        if (ParentIsForm)
            if (ParentForm.WindowState == FormWindowState.Maximized)
                return;

        if (Header.Contains(e.Location))
        {
            Flag = new IntPtr(2);
        }
        else if (Current.Position == 0 | !_Resizable)
        {
            return;
        }
        else
        {
            Flag = new IntPtr(Current.Position);
        }

        Capture = false;
        Message m = Message.Create(Parent.Handle, 161, Flag, IntPtr.Zero);
        DefWndProc(ref m);

        base.OnMouseDown(e);
    }

    private Pointer GetPointer()
    {
        PTC = PointToClient(MousePosition);
        F1 = PTC.X < 7;
        F2 = PTC.X > Width - 7;
        F3 = PTC.Y < 7;
        F4 = PTC.Y > Height - 7;

        if (F1 & F3)
            return new Pointer(Cursors.SizeNWSE, 13);
        if (F1 & F4)
            return new Pointer(Cursors.SizeNESW, 16);
        if (F2 & F3)
            return new Pointer(Cursors.SizeNESW, 14);
        if (F2 & F4)
            return new Pointer(Cursors.SizeNWSE, 17);
        if (F1)
            return new Pointer(Cursors.SizeWE, 10);
        if (F2)
            return new Pointer(Cursors.SizeWE, 11);
        if (F3)
            return new Pointer(Cursors.SizeNS, 12);
        if (F4)
            return new Pointer(Cursors.SizeNS, 15);
        return new Pointer(Cursors.Default, 0);
    }

    private void SetCurrent()
    {
        Pending = GetPointer();
        if (Current.Position == Pending.Position)
            return;
        Current = GetPointer();
        Cursor = Current.Cursor;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (_Resizable)
            SetCurrent();
        base.OnMouseMove(e);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;
        Header = new Rectangle(7, 7, Width - 14, _MoveHeight - 7);
        Invalidate();
        base.OnSizeChanged(e);
    }

    private struct Pointer
    {
        public readonly Cursor Cursor;
        public readonly byte Position;

        public Pointer(Cursor c, byte p)
        {
            Cursor = c;
            Position = p;
        }
    }

    #endregion

    #region " Convienence "

    private SolidBrush _Brush;
    private LinearGradientBrush _Gradient;
    private Image _Image;
    private Rectangle _Rectangle;
    private Size _Size;
    private Color _TransparencyKey;

    public Color TransparencyKey
    {
        get { return _TransparencyKey; }
        set
        {
            _TransparencyKey = value;
            Invalidate();
        }
    }

    public Image Image
    {
        get { return _Image; }
        set
        {
            _Image = value;
            Invalidate();
        }
    }

    public int ImageWidth
    {
        get
        {
            if (_Image == null)
                return 0;
            return _Image.Width;
        }
    }

    public abstract void PaintHook();

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;
        G = e.Graphics;
        PaintHook();
    }

    protected void DrawCorners(Color c, Rectangle rect)
    {
        _Brush = new SolidBrush(c);
        G.FillRectangle(_Brush, rect.X, rect.Y, 1, 1);
        G.FillRectangle(_Brush, rect.X + (rect.Width - 1), rect.Y, 1, 1);
        G.FillRectangle(_Brush, rect.X, rect.Y + (rect.Height - 1), 1, 1);
        G.FillRectangle(_Brush, rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), 1, 1);
    }

    protected void DrawBorders(Pen p1, Pen p2, Rectangle rect)
    {
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
    }

    protected void DrawText(HorizontalAlignment a, Color c, int x)
    {
        DrawText(a, c, x, 0);
    }

    protected void DrawText(HorizontalAlignment a, Color c, int x, int y)
    {
        if (string.IsNullOrEmpty(Text))
            return;
        _Size = G.MeasureString(Text, Font).ToSize();
        _Brush = new SolidBrush(c);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(Text, Font, _Brush, x, _MoveHeight / 2 - _Size.Height / 2 + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(Text, Font, _Brush, Width - _Size.Width - x, _MoveHeight / 2 - _Size.Height / 2 + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(Text, Font, _Brush, Width / 2 - _Size.Width / 2 + x, _MoveHeight / 2 - _Size.Height / 2 + y);
                break;
        }
    }

    protected void DrawIcon(HorizontalAlignment a, int x)
    {
        DrawIcon(a, x, 0);
    }

    protected void DrawIcon(HorizontalAlignment a, int x, int y)
    {
        if (_Image == null)
            return;
        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(_Image, x, _MoveHeight / 2 - _Image.Height / 2 + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(_Image, Width - _Image.Width - x, _MoveHeight / 2 - _Image.Height / 2 + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(_Image, Width / 2 - _Image.Width / 2, _MoveHeight / 2 - _Image.Height / 2);
                break;
        }
    }

    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        _Rectangle = new Rectangle(x, y, width, height);
        _Gradient = new LinearGradientBrush(_Rectangle, c1, c2, angle);
        G.FillRectangle(_Gradient, _Rectangle);
    }

    #endregion
}

internal static class Draw
{
    public static GraphicsPath RoundRect(Rectangle Rectangle, int Curve)
    {
        var P = new GraphicsPath();
        int ArcRectangleWidth = Curve * 2;
        P.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90);
        P.AddArc(
            new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth,
                ArcRectangleWidth), -90, 90);
        P.AddArc(
            new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X,
                Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90);
        P.AddArc(
            new Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth,
                ArcRectangleWidth), 90, 90);
        P.AddLine(new Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y),
            new Point(Rectangle.X, Curve + Rectangle.Y));
        return P;
    }

    public static void Gradient(Graphics g, Color c1, Color c2, int x, int y, int width, int height)
    {
        var R = new Rectangle(x, y, width, height);
        using (var T = new LinearGradientBrush(R, c1, c2, LinearGradientMode.Vertical))
        {
            g.FillRectangle(T, R);
        }
    }

    public static void Blend(Graphics g, Color c1, Color c2, Color c3, float c, int d, int x, int y, int width,
        int height)
    {
        var V = new ColorBlend(3);
        V.Colors = new[] { c1, c2, c3 };
        V.Positions = new[] { 0F, c, 1F };
        var R = new Rectangle(x, y, width, height);
        using (var T = new LinearGradientBrush(R, c1, c1, (LinearGradientMode)d))
        {
            T.InterpolationColors = V;
            g.FillRectangle(T, R);
        }
    }
}

internal abstract class ThemeControl : Control
{
    #region " Initialization "

    protected Bitmap B;
    protected Graphics G;

    public ThemeControl()
    {
        SetStyle((ControlStyles)139270, true);
        B = new Bitmap(1, 1);
        G = Graphics.FromImage(B);
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            Invalidate();
        }
    }

    public void AllowTransparent()
    {
        SetStyle(ControlStyles.Opaque, false);
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
    }

    #endregion

    #region " Mouse Handling "

    protected State MouseState;

    protected override void OnMouseLeave(EventArgs e)
    {
        ChangeMouseState(State.MouseNone);
        base.OnMouseLeave(e);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        ChangeMouseState(State.MouseOver);
        base.OnMouseEnter(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        ChangeMouseState(State.MouseOver);
        base.OnMouseUp(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            ChangeMouseState(State.MouseDown);
        base.OnMouseDown(e);
    }

    private void ChangeMouseState(State e)
    {
        MouseState = e;
        Invalidate();
    }

    protected enum State : byte
    {
        MouseNone = 0,
        MouseOver = 1,
        MouseDown = 2
    }

    #endregion

    #region " Convienence "

    private SolidBrush _Brush;
    private LinearGradientBrush _Gradient;
    private Image _Image;
    private bool _NoRounding;
    private Rectangle _Rectangle;
    private Size _Size;

    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    public Image Image
    {
        get { return _Image; }
        set
        {
            _Image = value;
            Invalidate();
        }
    }

    public int ImageWidth
    {
        get
        {
            if (_Image == null)
                return 0;
            return _Image.Width;
        }
    }

    public int ImageTop
    {
        get
        {
            if (_Image == null)
                return 0;
            return Height / 2 - _Image.Height / 2;
        }
    }

    public abstract void PaintHook();

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;
        PaintHook();
        e.Graphics.DrawImage(B, 0, 0);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        if (!(Width == 0) && !(Height == 0))
        {
            B = new Bitmap(Width, Height);
            G = Graphics.FromImage(B);
            Invalidate();
        }
        base.OnSizeChanged(e);
    }

    protected void DrawCorners(Color c, Rectangle rect)
    {
        if (_NoRounding)
            return;

        B.SetPixel(rect.X, rect.Y, c);
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c);
        B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c);
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c);
    }

    protected void DrawBorders(Pen p1, Pen p2, Rectangle rect)
    {
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
    }

    protected void DrawText(HorizontalAlignment a, Color c, int x)
    {
        DrawText(a, c, x, 0);
    }

    protected void DrawText(HorizontalAlignment a, Color c, int x, int y)
    {
        if (string.IsNullOrEmpty(Text))
            return;
        _Size = G.MeasureString(Text, Font).ToSize();
        _Brush = new SolidBrush(c);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(Text, Font, _Brush, x, Height / 2 - _Size.Height / 2 + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(Text, Font, _Brush, Width - _Size.Width - x, Height / 2 - _Size.Height / 2 + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(Text, Font, _Brush, Width / 2 - _Size.Width / 2 + x, Height / 2 - _Size.Height / 2 + y);
                break;
        }
    }

    protected void DrawIcon(HorizontalAlignment a, int x)
    {
        DrawIcon(a, x, 0);
    }

    protected void DrawIcon(HorizontalAlignment a, int x, int y)
    {
        if (_Image == null)
            return;
        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(_Image, x, Height / 2 - _Image.Height / 2 + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(_Image, Width - _Image.Width - x, Height / 2 - _Image.Height / 2 + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(_Image, Width / 2 - _Image.Width / 2, Height / 2 - _Image.Height / 2);
                break;
        }
    }

    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        _Rectangle = new Rectangle(x, y, width, height);
        _Gradient = new LinearGradientBrush(_Rectangle, c1, c2, angle);
        G.FillRectangle(_Gradient, _Rectangle);
    }

    #endregion
}

internal abstract class ThemeContainerControl : ContainerControl
{
    #region " Initialization "

    protected Bitmap B;
    protected Graphics G;

    public ThemeContainerControl()
    {
        SetStyle((ControlStyles)139270, true);
        B = new Bitmap(1, 1);
        G = Graphics.FromImage(B);
    }

    public void AllowTransparent()
    {
        SetStyle(ControlStyles.Opaque, false);
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
    }

    #endregion

    #region " Convienence "

    private LinearGradientBrush _Gradient;
    private bool _NoRounding;
    private Rectangle _Rectangle;

    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    public abstract void PaintHook();

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;
        PaintHook();
        e.Graphics.DrawImage(B, 0, 0);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        if (!(Width == 0) && !(Height == 0))
        {
            B = new Bitmap(Width, Height);
            G = Graphics.FromImage(B);
            Invalidate();
        }
        base.OnSizeChanged(e);
    }

    protected void DrawCorners(Color c, Rectangle rect)
    {
        if (_NoRounding)
            return;
        B.SetPixel(rect.X, rect.Y, c);
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c);
        B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c);
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c);
    }

    protected void DrawBorders(Pen p1, Pen p2, Rectangle rect)
    {
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
    }

    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        _Rectangle = new Rectangle(x, y, width, height);
        _Gradient = new LinearGradientBrush(_Rectangle, c1, c2, angle);
        G.FillRectangle(_Gradient, _Rectangle);
    }

    #endregion
}

#region themeBase1.53

internal abstract class ThemeContainer153 : ContainerControl
{
    #region " Initialization "

    protected Bitmap B;
    protected Graphics G;

    public ThemeContainer153()
    {
        SetStyle((ControlStyles)139270, true);

        _ImageSize = Size.Empty;
        Font = new Font("Verdana", 8);

        MeasureBitmap = new Bitmap(1, 1);
        MeasureGraphics = Graphics.FromImage(MeasureBitmap);

        DrawRadialPath = new GraphicsPath();

        InvalidateCustimization();
        //Remove?
    }

    protected override sealed void OnHandleCreated(EventArgs e)
    {
        InvalidateCustimization();
        ColorHook();

        if (!(_LockWidth == 0))
            Width = _LockWidth;
        if (!(_LockHeight == 0))
            Height = _LockHeight;
        if (!_ControlMode)
            base.Dock = DockStyle.Fill;

        Transparent = _Transparent;
        if (_Transparent && _BackColor)
            BackColor = Color.Transparent;

        base.OnHandleCreated(e);
    }

    protected override sealed void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);

        if (Parent == null)
            return;
        _IsParentForm = Parent is Form;

        if (!_ControlMode)
        {
            InitializeMessages();

            if (_IsParentForm)
            {
                ParentForm.FormBorderStyle = _BorderStyle;
                ParentForm.TransparencyKey = _TransparencyKey;
            }

            Parent.BackColor = BackColor;
        }

        OnCreation();
    }

    #endregion

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;

        if (_Transparent && _ControlMode)
        {
            PaintHook();
            e.Graphics.DrawImage(B, 0, 0);
        }
        else
        {
            G = e.Graphics;
            PaintHook();
        }
    }

    #region " Size Handling "

    private Rectangle Frame;

    protected override sealed void OnSizeChanged(EventArgs e)
    {
        if (_Movable && !_ControlMode)
        {
            Frame = new Rectangle(7, 7, Width - 14, _Header - 7);
        }

        InvalidateBitmap();
        Invalidate();

        base.OnSizeChanged(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        if (!(_LockWidth == 0))
            width = _LockWidth;
        if (!(_LockHeight == 0))
            height = _LockHeight;
        base.SetBoundsCore(x, y, width, height, specified);
    }

    #endregion

    #region " State Handling "

    private readonly Message[] Messages = new Message[9];
    private bool B1;
    private bool B2;
    private bool B3;
    private bool B4;
    private int Current;
    private Point GetIndexPoint;
    private int Previous;
    protected MouseState State;
    private bool WM_LMBUTTONDOWN;

    private void SetState(MouseState current)
    {
        State = current;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized))
        {
            if (_Sizable && !_ControlMode)
                InvalidateMouse();
        }

        base.OnMouseMove(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        if (Enabled)
            SetState(MouseState.None);
        else
            SetState(MouseState.Block);
        base.OnEnabledChanged(e);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        SetState(MouseState.Over);
        base.OnMouseEnter(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        SetState(MouseState.Over);
        base.OnMouseUp(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        SetState(MouseState.None);

        if (GetChildAtPoint(PointToClient(MousePosition)) != null)
        {
            if (_Sizable && !_ControlMode)
            {
                Cursor = Cursors.Default;
                Previous = 0;
            }
        }

        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            SetState(MouseState.Down);

        if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized || _ControlMode))
        {
            if (_Movable && Frame.Contains(e.Location))
            {
                Capture = false;
                WM_LMBUTTONDOWN = true;
                DefWndProc(ref Messages[0]);
            }
            else if (_Sizable && !(Previous == 0))
            {
                Capture = false;
                WM_LMBUTTONDOWN = true;
                DefWndProc(ref Messages[Previous]);
            }
        }

        base.OnMouseDown(e);
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if (WM_LMBUTTONDOWN && m.Msg == 513)
        {
            WM_LMBUTTONDOWN = false;

            SetState(MouseState.Over);
            if (!_SmartBounds)
                return;

            if (IsParentMdi)
            {
                CorrectBounds(new Rectangle(Point.Empty, Parent.Parent.Size));
            }
            else
            {
                CorrectBounds(Screen.FromControl(Parent).WorkingArea);
            }
        }
    }

    private int GetIndex()
    {
        GetIndexPoint = PointToClient(MousePosition);
        B1 = GetIndexPoint.X < 7;
        B2 = GetIndexPoint.X > Width - 7;
        B3 = GetIndexPoint.Y < 7;
        B4 = GetIndexPoint.Y > Height - 7;

        if (B1 && B3)
            return 4;
        if (B1 && B4)
            return 7;
        if (B2 && B3)
            return 5;
        if (B2 && B4)
            return 8;
        if (B1)
            return 1;
        if (B2)
            return 2;
        if (B3)
            return 3;
        if (B4)
            return 6;
        return 0;
    }

    private void InvalidateMouse()
    {
        Current = GetIndex();
        if (Current == Previous)
            return;

        Previous = Current;
        switch (Previous)
        {
            case 0:
                Cursor = Cursors.Default;
                break;
            case 1:
            case 2:
                Cursor = Cursors.SizeWE;
                break;
            case 3:
            case 6:
                Cursor = Cursors.SizeNS;
                break;
            case 4:
            case 8:
                Cursor = Cursors.SizeNWSE;
                break;
            case 5:
            case 7:
                Cursor = Cursors.SizeNESW;
                break;
        }
    }

    private void InitializeMessages()
    {
        Messages[0] = Message.Create(Parent.Handle, 161, new IntPtr(2), IntPtr.Zero);
        for (int I = 1; I <= 8; I++)
        {
            Messages[I] = Message.Create(Parent.Handle, 161, new IntPtr(I + 9), IntPtr.Zero);
        }
    }

    private void CorrectBounds(Rectangle bounds)
    {
        if (Parent.Width > bounds.Width)
            Parent.Width = bounds.Width;
        if (Parent.Height > bounds.Height)
            Parent.Height = bounds.Height;

        int X = Parent.Location.X;
        int Y = Parent.Location.Y;

        if (X < bounds.X)
            X = bounds.X;
        if (Y < bounds.Y)
            Y = bounds.Y;

        int Width = bounds.X + bounds.Width;
        int Height = bounds.Y + bounds.Height;

        if (X + Parent.Width > Width)
            X = Width - Parent.Width;
        if (Y + Parent.Height > Height)
            Y = Height - Parent.Height;

        Parent.Location = new Point(X, Y);
    }

    #endregion

    #region " Base Properties "

    private bool _BackColor;

    public override DockStyle Dock
    {
        get { return base.Dock; }
        set
        {
            if (!_ControlMode)
                return;
            base.Dock = value;
        }
    }

    [Category("Misc")]
    public override Color BackColor
    {
        get { return base.BackColor; }
        set
        {
            if (value == base.BackColor)
                return;

            if (!IsHandleCreated && _ControlMode && value == Color.Transparent)
            {
                _BackColor = true;
                return;
            }

            base.BackColor = value;
            if (Parent != null)
            {
                if (!_ControlMode)
                    Parent.BackColor = value;
                ColorHook();
            }
        }
    }

    public override Size MinimumSize
    {
        get { return base.MinimumSize; }
        set
        {
            base.MinimumSize = value;
            if (Parent != null)
                Parent.MinimumSize = value;
        }
    }

    public override Size MaximumSize
    {
        get { return base.MaximumSize; }
        set
        {
            base.MaximumSize = value;
            if (Parent != null)
                Parent.MaximumSize = value;
        }
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            Invalidate();
        }
    }

    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            Invalidate();
        }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color ForeColor
    {
        get { return Color.Empty; }
        set { }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Image BackgroundImage
    {
        get { return null; }
        set { }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ImageLayout BackgroundImageLayout
    {
        get { return ImageLayout.None; }
        set { }
    }

    #endregion

    #region " Public Properties "

    private readonly Dictionary<string, Color> Items = new Dictionary<string, Color>();
    private FormBorderStyle _BorderStyle;
    private string _Customization;
    private Image _Image;
    private bool _Movable = true;
    private bool _NoRounding;
    private bool _Sizable = true;
    private bool _SmartBounds = true;
    private Color _TransparencyKey;
    private bool _Transparent;

    public bool SmartBounds
    {
        get { return _SmartBounds; }
        set { _SmartBounds = value; }
    }

    public bool Movable
    {
        get { return _Movable; }
        set { _Movable = value; }
    }

    public bool Sizable
    {
        get { return _Sizable; }
        set { _Sizable = value; }
    }

    public Color TransparencyKey
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.TransparencyKey;
            return _TransparencyKey;
        }
        set
        {
            if (value == _TransparencyKey)
                return;
            _TransparencyKey = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.TransparencyKey = value;
                ColorHook();
            }
        }
    }

    public FormBorderStyle BorderStyle
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.FormBorderStyle;
            return _BorderStyle;
        }
        set
        {
            _BorderStyle = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.FormBorderStyle = value;

                if (!(value == FormBorderStyle.None))
                {
                    Movable = false;
                    Sizable = false;
                }
            }
        }
    }

    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    public Image Image
    {
        get { return _Image; }
        set
        {
            if (value == null)
                _ImageSize = Size.Empty;
            else
                _ImageSize = value.Size;

            _Image = value;
            Invalidate();
        }
    }

    public Bloom[] Colors
    {
        get
        {
            var T = new List<Bloom>();
            Dictionary<string, Color>.Enumerator E = Items.GetEnumerator();

            while (E.MoveNext())
            {
                T.Add(new Bloom(E.Current.Key, E.Current.Value));
            }

            return T.ToArray();
        }
        set
        {
            foreach (Bloom B in value)
            {
                if (Items.ContainsKey(B.Name))
                    Items[B.Name] = B.Value;
            }

            InvalidateCustimization();
            ColorHook();
            Invalidate();
        }
    }

    public string Customization
    {
        get { return _Customization; }
        set
        {
            if (value == _Customization)
                return;

            byte[] Data = null;
            Bloom[] Items = Colors;

            try
            {
                Data = Convert.FromBase64String(value);
                for (int I = 0; I <= Items.Length - 1; I++)
                {
                    Items[I].Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4));
                }
            }
            catch
            {
                return;
            }

            _Customization = value;

            Colors = Items;
            ColorHook();
            Invalidate();
        }
    }

    public bool Transparent
    {
        get { return _Transparent; }
        set
        {
            _Transparent = value;
            if (!(IsHandleCreated || _ControlMode))
                return;

            if (!value && !(BackColor.A == 255))
            {
                throw new Exception("Unable to change value to false while a transparent BackColor is in use.");
            }

            SetStyle(ControlStyles.Opaque, !value);
            SetStyle(ControlStyles.SupportsTransparentBackColor, value);

            InvalidateBitmap();
            Invalidate();
        }
    }

    #endregion

    #region " Private Properties "

    private bool _ControlMode;
    private int _Header = 24;
    private Size _ImageSize;

    private bool _IsParentForm;
    private int _LockHeight;
    private int _LockWidth;

    protected Size ImageSize
    {
        get { return _ImageSize; }
    }

    protected bool IsParentForm
    {
        get { return _IsParentForm; }
    }

    protected bool IsParentMdi
    {
        get
        {
            if (Parent == null)
                return false;
            return Parent.Parent != null;
        }
    }

    protected int LockWidth
    {
        get { return _LockWidth; }
        set
        {
            _LockWidth = value;
            if (!(LockWidth == 0) && IsHandleCreated)
                Width = LockWidth;
        }
    }

    protected int LockHeight
    {
        get { return _LockHeight; }
        set
        {
            _LockHeight = value;
            if (!(LockHeight == 0) && IsHandleCreated)
                Height = LockHeight;
        }
    }

    protected int Header
    {
        get { return _Header; }
        set
        {
            _Header = value;

            if (!_ControlMode)
            {
                Frame = new Rectangle(7, 7, Width - 14, value - 7);
                Invalidate();
            }
        }
    }

    protected bool ControlMode
    {
        get { return _ControlMode; }
        set
        {
            _ControlMode = value;

            Transparent = _Transparent;
            if (_Transparent && _BackColor)
                BackColor = Color.Transparent;

            InvalidateBitmap();
            Invalidate();
        }
    }

    #endregion

    #region " Property Helpers "

    protected Pen GetPen(string name)
    {
        return new Pen(Items[name]);
    }

    protected Pen GetPen(string name, float width)
    {
        return new Pen(Items[name], width);
    }

    protected SolidBrush GetBrush(string name)
    {
        return new SolidBrush(Items[name]);
    }

    protected Color GetColor(string name)
    {
        return Items[name];
    }

    protected void SetColor(string name, Color value)
    {
        if (Items.ContainsKey(name))
            Items[name] = value;
        else
            Items.Add(name, value);
    }

    protected void SetColor(string name, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(r, g, b));
    }

    protected void SetColor(string name, byte a, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(a, r, g, b));
    }

    protected void SetColor(string name, byte a, Color value)
    {
        SetColor(name, Color.FromArgb(a, value));
    }

    private void InvalidateBitmap()
    {
        if (_Transparent && _ControlMode)
        {
            if (Width == 0 || Height == 0)
                return;
            B = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
            G = Graphics.FromImage(B);
        }
        else
        {
            G = null;
            B = null;
        }
    }

    private void InvalidateCustimization()
    {
        var M = new MemoryStream(Items.Count * 4);

        foreach (Bloom B in Colors)
        {
            M.Write(BitConverter.GetBytes(B.Value.ToArgb()), 0, 4);
        }

        M.Close();
        _Customization = Convert.ToBase64String(M.ToArray());
    }

    #endregion

    #region " User Hooks "

    protected abstract void ColorHook();
    protected abstract void PaintHook();

    protected virtual void OnCreation()
    {
    }

    #endregion

    #region " Offset "

    private Point OffsetReturnPoint;
    private Rectangle OffsetReturnRectangle;

    private Size OffsetReturnSize;

    protected Rectangle Offset(Rectangle r, int amount)
    {
        OffsetReturnRectangle = new Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2));
        return OffsetReturnRectangle;
    }

    protected Size Offset(Size s, int amount)
    {
        OffsetReturnSize = new Size(s.Width + amount, s.Height + amount);
        return OffsetReturnSize;
    }

    protected Point Offset(Point p, int amount)
    {
        OffsetReturnPoint = new Point(p.X + amount, p.Y + amount);
        return OffsetReturnPoint;
    }

    #endregion

    #region " Center "

    private Point CenterReturn;

    protected Point Center(Rectangle p, Rectangle c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X + c.X, (p.Height / 2 - c.Height / 2) + p.Y + c.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle p, Size c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X, (p.Height / 2 - c.Height / 2) + p.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }

    protected Point Center(Size child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }

    protected Point Center(int childWidth, int childHeight)
    {
        return Center(Width, Height, childWidth, childHeight);
    }

    protected Point Center(Size p, Size c)
    {
        return Center(p.Width, p.Height, c.Width, c.Height);
    }

    protected Point Center(int pWidth, int pHeight, int cWidth, int cHeight)
    {
        CenterReturn = new Point(pWidth / 2 - cWidth / 2, pHeight / 2 - cHeight / 2);
        return CenterReturn;
    }

    #endregion

    #region " Measure "

    private readonly Graphics MeasureGraphics;
    private Bitmap MeasureBitmap;

    protected Size Measure()
    {
        return MeasureGraphics.MeasureString(Text, Font, Width).ToSize();
    }

    protected Size Measure(string text)
    {
        return MeasureGraphics.MeasureString(text, Font, Width).ToSize();
    }

    #endregion

    #region " DrawPixel "

    private SolidBrush DrawPixelBrush;

    protected void DrawPixel(Color c1, int x, int y)
    {
        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
        }
        else
        {
            DrawPixelBrush = new SolidBrush(c1);
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1);
        }
    }

    #endregion

    #region " DrawCorners "

    private SolidBrush DrawCornersBrush;

    protected void DrawCorners(Color c1, int offset)
    {
        DrawCorners(c1, 0, 0, Width, Height, offset);
    }

    protected void DrawCorners(Color c1, Rectangle r1, int offset)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset);
    }

    protected void DrawCorners(Color c1, int x, int y, int width, int height, int offset)
    {
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawCorners(Color c1)
    {
        DrawCorners(c1, 0, 0, Width, Height);
    }

    protected void DrawCorners(Color c1, Rectangle r1)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height);
    }

    protected void DrawCorners(Color c1, int x, int y, int width, int height)
    {
        if (_NoRounding)
            return;

        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
            B.SetPixel(x + (width - 1), y, c1);
            B.SetPixel(x, y + (height - 1), c1);
            B.SetPixel(x + (width - 1), y + (height - 1), c1);
        }
        else
        {
            DrawCornersBrush = new SolidBrush(c1);
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1);
        }
    }

    #endregion

    #region " DrawBorders "

    protected void DrawBorders(Pen p1, int offset)
    {
        DrawBorders(p1, 0, 0, Width, Height, offset);
    }

    protected void DrawBorders(Pen p1, Rectangle r, int offset)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset);
    }

    protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset)
    {
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawBorders(Pen p1)
    {
        DrawBorders(p1, 0, 0, Width, Height);
    }

    protected void DrawBorders(Pen p1, Rectangle r)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height);
    }

    protected void DrawBorders(Pen p1, int x, int y, int width, int height)
    {
        G.DrawRectangle(p1, x, y, width - 1, height - 1);
    }

    #endregion

    #region " DrawText "

    private Point DrawTextPoint;

    private Size DrawTextSize;

    protected void DrawText(Brush b1, HorizontalAlignment a, int x, int y)
    {
        DrawText(b1, Text, a, x, y);
    }

    protected void DrawText(Brush b1, string text, HorizontalAlignment a, int x, int y)
    {
        if (text.Length == 0)
            return;

        DrawTextSize = Measure(text);
        DrawTextPoint = new Point(Width / 2 - DrawTextSize.Width / 2, Header / 2 - DrawTextSize.Height / 2);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y);
                break;
        }
    }

    protected void DrawText(Brush b1, Point p1)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, p1);
    }

    protected void DrawText(Brush b1, int x, int y)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, x, y);
    }

    #endregion

    #region " DrawImage "

    private Point DrawImagePoint;

    protected void DrawImage(HorizontalAlignment a, int x, int y)
    {
        DrawImage(_Image, a, x, y);
    }

    protected void DrawImage(Image image, HorizontalAlignment a, int x, int y)
    {
        if (image == null)
            return;
        DrawImagePoint = new Point(Width / 2 - image.Width / 2, Header / 2 - image.Height / 2);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
        }
    }

    protected void DrawImage(Point p1)
    {
        DrawImage(_Image, p1.X, p1.Y);
    }

    protected void DrawImage(int x, int y)
    {
        DrawImage(_Image, x, y);
    }

    protected void DrawImage(Image image, Point p1)
    {
        DrawImage(image, p1.X, p1.Y);
    }

    protected void DrawImage(Image image, int x, int y)
    {
        if (image == null)
            return;
        G.DrawImage(image, x, y, image.Width, image.Height);
    }

    #endregion

    #region " DrawGradient "

    private LinearGradientBrush DrawGradientBrush;

    private Rectangle DrawGradientRectangle;

    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle);
    }

    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, 90f);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, angle);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }


    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle);
    }

    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, angle);
        G.FillRectangle(DrawGradientBrush, r);
    }

    #endregion

    #region " DrawRadial "

    private readonly GraphicsPath DrawRadialPath;
    private PathGradientBrush DrawRadialBrush1;
    private LinearGradientBrush DrawRadialBrush2;

    private Rectangle DrawRadialRectangle;

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, width / 2, height / 2);
    }

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, Point center)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y);
    }

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, int cx, int cy)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, cx, cy);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r)
    {
        DrawRadial(blend, r, r.Width / 2, r.Height / 2);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r, Point center)
    {
        DrawRadial(blend, r, center.X, center.Y);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r, int cx, int cy)
    {
        DrawRadialPath.Reset();
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1);

        DrawRadialBrush1 = new PathGradientBrush(DrawRadialPath);
        DrawRadialBrush1.CenterPoint = new Point(r.X + cx, r.Y + cy);
        DrawRadialBrush1.InterpolationColors = blend;

        if (G.SmoothingMode == SmoothingMode.AntiAlias)
        {
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3);
        }
        else
        {
            G.FillEllipse(DrawRadialBrush1, r);
        }
    }


    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawGradientRectangle);
    }

    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, angle);
        G.FillEllipse(DrawGradientBrush, r);
    }

    #endregion
}

#endregion

internal abstract class ThemeControl153 : Control
{
    #region " Initialization "

    protected Bitmap B;
    protected Graphics G;

    public ThemeControl153()
    {
        SetStyle((ControlStyles)139270, true);

        _ImageSize = Size.Empty;
        Font = new Font("Verdana", 8);

        MeasureBitmap = new Bitmap(1, 1);
        MeasureGraphics = Graphics.FromImage(MeasureBitmap);

        DrawRadialPath = new GraphicsPath();

        InvalidateCustimization();
        //Remove?
    }

    protected override sealed void OnHandleCreated(EventArgs e)
    {
        InvalidateCustimization();
        ColorHook();

        if (!(_LockWidth == 0))
            Width = _LockWidth;
        if (!(_LockHeight == 0))
            Height = _LockHeight;

        Transparent = _Transparent;
        if (_Transparent && _BackColor)
            BackColor = Color.Transparent;

        base.OnHandleCreated(e);
    }

    protected override sealed void OnParentChanged(EventArgs e)
    {
        if (Parent != null)
            OnCreation();
        base.OnParentChanged(e);
    }

    #endregion

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;

        if (_Transparent)
        {
            PaintHook();
            e.Graphics.DrawImage(B, 0, 0);
        }
        else
        {
            G = e.Graphics;
            PaintHook();
        }
    }

    #region " Size Handling "

    protected override sealed void OnSizeChanged(EventArgs e)
    {
        if (_Transparent)
        {
            InvalidateBitmap();
        }

        Invalidate();
        base.OnSizeChanged(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        if (!(_LockWidth == 0))
            width = _LockWidth;
        if (!(_LockHeight == 0))
            height = _LockHeight;
        base.SetBoundsCore(x, y, width, height, specified);
    }

    #endregion

    #region " State Handling "

    private bool InPosition;
    protected MouseState State;

    protected override void OnMouseEnter(EventArgs e)
    {
        InPosition = true;
        SetState(MouseState.Over);
        base.OnMouseEnter(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (InPosition)
            SetState(MouseState.Over);
        base.OnMouseUp(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            SetState(MouseState.Down);
        base.OnMouseDown(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        InPosition = false;
        SetState(MouseState.None);
        base.OnMouseLeave(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        if (Enabled)
            SetState(MouseState.None);
        else
            SetState(MouseState.Block);
        base.OnEnabledChanged(e);
    }

    private void SetState(MouseState current)
    {
        State = current;
        Invalidate();
    }

    #endregion

    #region " Base Properties "

    private bool _BackColor;

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color ForeColor
    {
        get { return Color.Empty; }
        set { }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Image BackgroundImage
    {
        get { return null; }
        set { }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ImageLayout BackgroundImageLayout
    {
        get { return ImageLayout.None; }
        set { }
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            Invalidate();
        }
    }

    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            Invalidate();
        }
    }

    [Category("Misc")]
    public override Color BackColor
    {
        get { return base.BackColor; }
        set
        {
            if (!IsHandleCreated && value == Color.Transparent)
            {
                _BackColor = true;
                return;
            }

            base.BackColor = value;
            if (Parent != null)
                ColorHook();
        }
    }

    #endregion

    #region " Public Properties "

    private readonly Dictionary<string, Color> Items = new Dictionary<string, Color>();
    private string _Customization;
    private Image _Image;
    private bool _NoRounding;
    private bool _Transparent;

    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    public Image Image
    {
        get { return _Image; }
        set
        {
            if (value == null)
            {
                _ImageSize = Size.Empty;
            }
            else
            {
                _ImageSize = value.Size;
            }

            _Image = value;
            Invalidate();
        }
    }

    public bool Transparent
    {
        get { return _Transparent; }
        set
        {
            _Transparent = value;
            if (!IsHandleCreated)
                return;

            if (!value && !(BackColor.A == 255))
            {
                throw new Exception("Unable to change value to false while a transparent BackColor is in use.");
            }

            SetStyle(ControlStyles.Opaque, !value);
            SetStyle(ControlStyles.SupportsTransparentBackColor, value);

            if (value)
                InvalidateBitmap();
            else
                B = null;
            Invalidate();
        }
    }

    public Bloom[] Colors
    {
        get
        {
            var T = new List<Bloom>();
            Dictionary<string, Color>.Enumerator E = Items.GetEnumerator();

            while (E.MoveNext())
            {
                T.Add(new Bloom(E.Current.Key, E.Current.Value));
            }

            return T.ToArray();
        }
        set
        {
            foreach (Bloom B in value)
            {
                if (Items.ContainsKey(B.Name))
                    Items[B.Name] = B.Value;
            }

            InvalidateCustimization();
            ColorHook();
            Invalidate();
        }
    }

    public string Customization
    {
        get { return _Customization; }
        set
        {
            if (value == _Customization)
                return;

            byte[] Data = null;
            Bloom[] Items = Colors;

            try
            {
                Data = Convert.FromBase64String(value);
                for (int I = 0; I <= Items.Length - 1; I++)
                {
                    Items[I].Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4));
                }
            }
            catch
            {
                return;
            }

            _Customization = value;

            Colors = Items;
            ColorHook();
            Invalidate();
        }
    }

    #endregion

    #region " Private Properties "

    private Size _ImageSize;
    private int _LockHeight;

    private int _LockWidth;

    protected Size ImageSize
    {
        get { return _ImageSize; }
    }

    protected int LockWidth
    {
        get { return _LockWidth; }
        set
        {
            _LockWidth = value;
            if (!(LockWidth == 0) && IsHandleCreated)
                Width = LockWidth;
        }
    }

    protected int LockHeight
    {
        get { return _LockHeight; }
        set
        {
            _LockHeight = value;
            if (!(LockHeight == 0) && IsHandleCreated)
                Height = LockHeight;
        }
    }

    #endregion

    #region " Property Helpers "

    protected Pen GetPen(string name)
    {
        return new Pen(Items[name]);
    }

    protected Pen GetPen(string name, float width)
    {
        return new Pen(Items[name], width);
    }

    protected SolidBrush GetBrush(string name)
    {
        return new SolidBrush(Items[name]);
    }

    protected Color GetColor(string name)
    {
        return Items[name];
    }

    protected void SetColor(string name, Color value)
    {
        if (Items.ContainsKey(name))
            Items[name] = value;
        else
            Items.Add(name, value);
    }

    protected void SetColor(string name, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(r, g, b));
    }

    protected void SetColor(string name, byte a, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(a, r, g, b));
    }

    protected void SetColor(string name, byte a, Color value)
    {
        SetColor(name, Color.FromArgb(a, value));
    }

    private void InvalidateBitmap()
    {
        if (Width == 0 || Height == 0)
            return;
        B = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
        G = Graphics.FromImage(B);
    }

    private void InvalidateCustimization()
    {
        var M = new MemoryStream(Items.Count * 4);

        foreach (Bloom B in Colors)
        {
            M.Write(BitConverter.GetBytes(B.Value.ToArgb()), 0, 4);
        }

        M.Close();
        _Customization = Convert.ToBase64String(M.ToArray());
    }

    #endregion

    #region " User Hooks "

    protected abstract void ColorHook();
    protected abstract void PaintHook();

    protected virtual void OnCreation()
    {
    }

    #endregion

    #region " Offset "

    private Point OffsetReturnPoint;
    private Rectangle OffsetReturnRectangle;

    private Size OffsetReturnSize;

    protected Rectangle Offset(Rectangle r, int amount)
    {
        OffsetReturnRectangle = new Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2));
        return OffsetReturnRectangle;
    }

    protected Size Offset(Size s, int amount)
    {
        OffsetReturnSize = new Size(s.Width + amount, s.Height + amount);
        return OffsetReturnSize;
    }

    protected Point Offset(Point p, int amount)
    {
        OffsetReturnPoint = new Point(p.X + amount, p.Y + amount);
        return OffsetReturnPoint;
    }

    #endregion

    #region " Center "

    private Point CenterReturn;

    protected Point Center(Rectangle p, Rectangle c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X + c.X, (p.Height / 2 - c.Height / 2) + p.Y + c.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle p, Size c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X, (p.Height / 2 - c.Height / 2) + p.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }

    protected Point Center(Size child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }

    protected Point Center(int childWidth, int childHeight)
    {
        return Center(Width, Height, childWidth, childHeight);
    }

    protected Point Center(Size p, Size c)
    {
        return Center(p.Width, p.Height, c.Width, c.Height);
    }

    protected Point Center(int pWidth, int pHeight, int cWidth, int cHeight)
    {
        CenterReturn = new Point(pWidth / 2 - cWidth / 2, pHeight / 2 - cHeight / 2);
        return CenterReturn;
    }

    #endregion

    #region " Measure "

    private readonly Graphics MeasureGraphics;
    private Bitmap MeasureBitmap;

    protected Size Measure()
    {
        return MeasureGraphics.MeasureString(Text, Font, Width).ToSize();
    }

    protected Size Measure(string text)
    {
        return MeasureGraphics.MeasureString(text, Font, Width).ToSize();
    }

    #endregion

    #region " DrawPixel "

    private SolidBrush DrawPixelBrush;

    protected void DrawPixel(Color c1, int x, int y)
    {
        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
        }
        else
        {
            DrawPixelBrush = new SolidBrush(c1);
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1);
        }
    }

    #endregion

    #region " DrawCorners "

    private SolidBrush DrawCornersBrush;

    protected void DrawCorners(Color c1, int offset)
    {
        DrawCorners(c1, 0, 0, Width, Height, offset);
    }

    protected void DrawCorners(Color c1, Rectangle r1, int offset)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset);
    }

    protected void DrawCorners(Color c1, int x, int y, int width, int height, int offset)
    {
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawCorners(Color c1)
    {
        DrawCorners(c1, 0, 0, Width, Height);
    }

    protected void DrawCorners(Color c1, Rectangle r1)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height);
    }

    protected void DrawCorners(Color c1, int x, int y, int width, int height)
    {
        if (_NoRounding)
            return;

        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
            B.SetPixel(x + (width - 1), y, c1);
            B.SetPixel(x, y + (height - 1), c1);
            B.SetPixel(x + (width - 1), y + (height - 1), c1);
        }
        else
        {
            DrawCornersBrush = new SolidBrush(c1);
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1);
        }
    }

    #endregion

    #region " DrawBorders "

    protected void DrawBorders(Pen p1, int offset)
    {
        DrawBorders(p1, 0, 0, Width, Height, offset);
    }

    protected void DrawBorders(Pen p1, Rectangle r, int offset)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset);
    }

    protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset)
    {
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawBorders(Pen p1)
    {
        DrawBorders(p1, 0, 0, Width, Height);
    }

    protected void DrawBorders(Pen p1, Rectangle r)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height);
    }

    protected void DrawBorders(Pen p1, int x, int y, int width, int height)
    {
        G.DrawRectangle(p1, x, y, width - 1, height - 1);
    }

    #endregion

    #region " DrawText "

    private Point DrawTextPoint;

    private Size DrawTextSize;

    protected void DrawText(Brush b1, HorizontalAlignment a, int x, int y)
    {
        DrawText(b1, Text, a, x, y);
    }

    protected void DrawText(Brush b1, string text, HorizontalAlignment a, int x, int y)
    {
        if (text.Length == 0)
            return;

        DrawTextSize = Measure(text);
        DrawTextPoint = Center(DrawTextSize);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y);
                break;
        }
    }

    protected void DrawText(Brush b1, Point p1)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, p1);
    }

    protected void DrawText(Brush b1, int x, int y)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, x, y);
    }

    #endregion

    #region " DrawImage "

    private Point DrawImagePoint;

    protected void DrawImage(HorizontalAlignment a, int x, int y)
    {
        DrawImage(_Image, a, x, y);
    }

    protected void DrawImage(Image image, HorizontalAlignment a, int x, int y)
    {
        if (image == null)
            return;
        DrawImagePoint = Center(image.Size);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
        }
    }

    protected void DrawImage(Point p1)
    {
        DrawImage(_Image, p1.X, p1.Y);
    }

    protected void DrawImage(int x, int y)
    {
        DrawImage(_Image, x, y);
    }

    protected void DrawImage(Image image, Point p1)
    {
        DrawImage(image, p1.X, p1.Y);
    }

    protected void DrawImage(Image image, int x, int y)
    {
        if (image == null)
            return;
        G.DrawImage(image, x, y, image.Width, image.Height);
    }

    #endregion

    #region " DrawGradient "

    private LinearGradientBrush DrawGradientBrush;

    private Rectangle DrawGradientRectangle;

    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle);
    }

    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, 90f);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, angle);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }


    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle);
    }

    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, angle);
        G.FillRectangle(DrawGradientBrush, r);
    }

    #endregion

    #region " DrawRadial "

    private readonly GraphicsPath DrawRadialPath;
    private PathGradientBrush DrawRadialBrush1;
    private LinearGradientBrush DrawRadialBrush2;

    private Rectangle DrawRadialRectangle;

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, width / 2, height / 2);
    }

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, Point center)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y);
    }

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, int cx, int cy)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, cx, cy);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r)
    {
        DrawRadial(blend, r, r.Width / 2, r.Height / 2);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r, Point center)
    {
        DrawRadial(blend, r, center.X, center.Y);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r, int cx, int cy)
    {
        DrawRadialPath.Reset();
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1);

        DrawRadialBrush1 = new PathGradientBrush(DrawRadialPath);
        DrawRadialBrush1.CenterPoint = new Point(r.X + cx, r.Y + cy);
        DrawRadialBrush1.InterpolationColors = blend;

        if (G.SmoothingMode == SmoothingMode.AntiAlias)
        {
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3);
        }
        else
        {
            G.FillEllipse(DrawRadialBrush1, r);
        }
    }


    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawRadialRectangle);
    }

    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawRadialRectangle, angle);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillEllipse(DrawRadialBrush2, r);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, angle);
        G.FillEllipse(DrawRadialBrush2, r);
    }

    #endregion
}

#endregion

//Basecolors:
//  Dark:
//      -> BG:          Color.FromArgb(35, 35, 35);
//      -> FG:          Color.FromArgb(50, 50, 50); 
//      -> Highlight:   Color.FromArgb(100, 100, 100);
//      -> Shadow:      Color.FromArgb(255, 168, 0);
//      -> Stripes:     Color.FromArgb(50, 50, 50);
//  Bright:
//      -> BG           Color.FromArgb(244, 244, 244);
//      -> FG           Color.FromArgb(170, 170, 170)
//      -> Highlight:   Color.FromArgb(220, 220, 220)
//      -> Shadow:      Color.FromArgb(255, 255, 255)
//      -> Highlight2:  Color.FromArgb(248, 248, 248)

#region Basis MeteTheme Elemente

internal class MeteThemeDark : Control
{
    private readonly SolidBrush B1;
    private readonly Color C1;
    private readonly Color C3;
    private readonly Pen P1;
    private readonly Pen P2;
    private readonly Pen P3;
    private readonly Pen P4;
    private Bitmap B;
    private LinearGradientBrush B2;
    private LinearGradientBrush B3;
    private Color C2;
    private Graphics G;
    private Rectangle R1;
    private Rectangle R2;
    public Color farbCode1 = Color.FromArgb(50, 50, 50); //Schrift/Vordergrundfarbe
    public Color farbCode2 = Color.FromArgb(35, 35, 35); //Hintergrundfarbe
    public Color farbCode3 = Color.FromArgb(100, 100, 100); //Highlight
    public Color farbCode4 = Color.FromArgb(255, 168, 0); //Schattenfarbe
    public Color farbCode5 = Color.FromArgb(35, 35, 35); //Rand-Farbe (Border)
    public Color farbCode6 = Color.FromArgb(50, 50, 50); //Diagonale Streifen

    public MeteThemeDark()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        C1 = farbCode2;
        C2 = farbCode3;
        C3 = farbCode4;
        P1 = new Pen(farbCode5);
        P4 = new Pen(farbCode6);
        P2 = new Pen(C1);
        P3 = new Pen(C2);
        B1 = new SolidBrush(farbCode1);
        Font = new Font("Arial", 11.0F);
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        Dock = DockStyle.Fill;
        if (Parent is Form)
        {
            var tempWith1 = (Form)Parent;
            tempWith1.FormBorderStyle = 0;
            tempWith1.BackColor = C1;
            tempWith1.ForeColor = farbCode1;
        }
        base.OnHandleCreated(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (
            new Rectangle(Parent.Location.X, Parent.Location.Y, Width, 22).IntersectsWith(new Rectangle(
                MousePosition.X, MousePosition.Y, 1, 1)))
        {
            Capture = false;
            Message M = Message.Create(Parent.Handle, 161, new IntPtr(2), IntPtr.Zero);
            DefWndProc(ref M);
        }
        base.OnMouseDown(e);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        if (Height > 0)
        {
            R1 = new Rectangle(0, 2, Width, 18);
            R2 = new Rectangle(0, 21, Width, 10);
            B2 = new LinearGradientBrush(R1, C1, C3, 90.0F);
            B3 = new LinearGradientBrush(R2, Color.FromArgb(18, 0, 0, 0), Color.Transparent, 90.0F);
            Invalidate();
        }
        base.OnSizeChanged(e);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        B = new Bitmap(Width, Height);
        G = Graphics.FromImage(B);

        G.Clear(C1);

        for (int I = 0; I <= Width + 17; I += 4)
        {
            G.DrawLine(P4, I, 21, I - 17, 37);
            G.DrawLine(P4, I - 1, 21, I - 16, 37);
        }
        G.FillRectangle(B3, R2);

        G.FillRectangle(B2, R1);
        G.DrawString(Text, Font, B1, 5, 5);

        G.DrawRectangle(P2, 1, 1, Width - 3, 19);
        G.DrawRectangle(P3, 1, 39, Width - 3, Height - 41);

        G.DrawRectangle(P1, 0, 0, Width - 1, Height - 1);
        G.DrawLine(P1, 0, 21, Width, 21);
        G.DrawLine(P1, 0, 38, Width, 38);

        e.Graphics.DrawImage(B, 0, 0);
        G.Dispose();
        B.Dispose();
    }
}

internal class MeteButton_Bright : Control
{
    private readonly Brush B1;
    private readonly Brush B2;
    private readonly Brush B5;
    private readonly Color C2;
    private readonly Color C3;
    private readonly Color C4;
    private readonly Pen P1;
    private readonly Pen P4;
    private Bitmap B;
    private LinearGradientBrush B3;
    private LinearGradientBrush B4;
    private Color C1;
    private Graphics G;
    private bool ImageSet;
    private Pen P2;
    private Pen P3;
    private Rectangle R1;

    private int State;
    private Image _Image;

    public MeteButton_Bright()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        C1 = Color.FromArgb(244, 244, 244); //Background
        C2 = Color.FromArgb(220, 220, 220); //Highlight
        C3 = Color.FromArgb(248, 248, 248); //Lesser Highlight
        C4 = Color.FromArgb(24, Color.Black);
        P1 = new Pen(Color.FromArgb(255, 255, 255)); //Shadow
        P2 = new Pen(Color.FromArgb(40, Color.White));
        P3 = new Pen(Color.FromArgb(20, Color.White));
        P4 = new Pen(Color.FromArgb(10, Color.Black)); //Down-Left
        B1 = new SolidBrush(C1);
        B2 = new SolidBrush(C3);
        B5 = new SolidBrush(Color.FromArgb(170, 170, 170)); //Text Color
        Font = new Font("Verdana", 8.0F);
    }

    public Image Image
    {
        get { return _Image; }
        set
        {
            _Image = value;
            ImageSet = value != null;
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        State = 0;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        State = 1;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        State = 1;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        State = 2;
        Invalidate();
    }

    protected override void OnResize(EventArgs e)
    {
        R1 = new Rectangle(2, 2, Width - 4, 4);
        B3 = new LinearGradientBrush(ClientRectangle, C3, C2, 90.0F);
        B4 = new LinearGradientBrush(R1, C4, Color.Transparent, 90.0F);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        B = new Bitmap(Width, Height);
        G = Graphics.FromImage(B);

        G.FillRectangle(B3, ClientRectangle);

        switch (State)
        {
            case 0: //Up
                G.FillRectangle(B2, 1, 1, Width - 2, Height - 2);
                G.DrawRectangle(P4, 2, 2, Width - 5, Height - 5);
                break;
            case 1: //Over
                G.FillRectangle(B1, 1, 1, Width - 2, Height - 2);
                G.DrawRectangle(P4, 2, 2, Width - 5, Height - 5);
                break;
            case 2: //Down
                G.FillRectangle(B1, 1, 1, Width - 2, Height - 2);
                G.FillRectangle(B4, R1);
                G.DrawLine(P4, 2, 2, 2, Height - 3);
                break;
        }

        SizeF S = G.MeasureString(Text, Font);
        G.DrawString(Text, Font, B5, Convert.ToInt32(Width / 2 - S.Width / 2.0), Convert.ToInt32(Height / 2 - S.Height / 2.0));

        G.DrawRectangle(P1, 1, 1, Width - 3, Height - 3);

        if (ImageSet)
        {
            G.DrawImage(_Image, 5, Convert.ToInt32(Height / 2 - _Image.Height / 2.0), _Image.Width, _Image.Height);
        }

        e.Graphics.DrawImage(B, 0, 0);
        G.Dispose();
        B.Dispose();
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
    }
}

internal class MeteButton_Dark : Control
{
    private readonly Brush B1;
    private readonly Brush B2;
    private readonly Brush B5;
    private readonly Color C2;
    private readonly Color C3;
    private readonly Color C4;
    private readonly Pen P1;
    private readonly Pen P4;
    private Bitmap B;
    private LinearGradientBrush B3;
    private LinearGradientBrush B4;
    private Color C1;
    private Graphics G;
    private bool ImageSet;
    private Pen P2;
    private Pen P3;
    private Rectangle R1;

    private int State;
    private Image _Image;

    public MeteButton_Dark()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        C1 = Color.FromArgb(35, 35, 35); //Background
        C2 = Color.FromArgb(120, 120, 120); //Highlight
        C3 = Color.FromArgb(90, 90, 90); //Lesser Highlight
        C4 = Color.FromArgb(24, Color.Black);
        P1 = new Pen(Color.FromArgb(30, 30, 30)); //Shadow
        P2 = new Pen(Color.FromArgb(40, Color.White));
        P3 = new Pen(Color.FromArgb(20, Color.White));
        P4 = new Pen(Color.FromArgb(10, Color.Black)); //Down-Left
        B1 = new SolidBrush(C1);
        B2 = new SolidBrush(C3);
        B5 = new SolidBrush(Color.FromArgb(255, 168, 0)); //Text Color
        Font = new Font("Verdana", 8.0F);
    }

    public Image Image
    {
        get { return _Image; }
        set
        {
            _Image = value;
            ImageSet = value != null;
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        State = 0;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        State = 1;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        State = 1;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        State = 2;
        Invalidate();
    }

    protected override void OnResize(EventArgs e)
    {
        R1 = new Rectangle(2, 2, Width - 4, 4);
        B3 = new LinearGradientBrush(ClientRectangle, C3, C2, 90.0F);
        B4 = new LinearGradientBrush(R1, C4, Color.Transparent, 90.0F);
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        B = new Bitmap(Width, Height);
        G = Graphics.FromImage(B);

        G.FillRectangle(B3, ClientRectangle);

        switch (State)
        {
            case 0: //Up
                G.FillRectangle(B2, 1, 1, Width - 2, Height - 2);
                G.DrawRectangle(P4, 2, 2, Width - 5, Height - 5);
                break;
            case 1: //Over
                G.FillRectangle(B1, 1, 1, Width - 2, Height - 2);
                G.DrawRectangle(P4, 2, 2, Width - 5, Height - 5);
                break;
            case 2: //Down
                G.FillRectangle(B1, 1, 1, Width - 2, Height - 2);
                G.FillRectangle(B4, R1);
                G.DrawLine(P4, 2, 2, 2, Height - 3);
                break;
        }

        SizeF S = G.MeasureString(Text, Font);
        G.DrawString(Text, Font, B5, Convert.ToInt32(Width / 2 - S.Width / 2.0), Convert.ToInt32(Height / 2 - S.Height / 2.0));

        G.DrawRectangle(P1, 1, 1, Width - 3, Height - 3);

        if (ImageSet)
        {
            G.DrawImage(_Image, 5, Convert.ToInt32(Height / 2 - _Image.Height / 2.0), _Image.Width, _Image.Height);
        }

        e.Graphics.DrawImage(B, 0, 0);
        G.Dispose();
        B.Dispose();
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
    }
}

internal class MeteProgressBarBright : Control
{
    #region  Properties

    private Color C2 = Color.FromArgb(232, 232, 232); //Dark Color
    private Color C3 = Color.AliceBlue; //Light color
    private double _Current;
    private double _Maximum = 100;
    private int _Progress;

    public double Maximum
    {
        get { return _Maximum; }
        set
        {
            _Maximum = value;
            Progress = _Current / value * 100;
            Invalidate();
        }
    }

    public double Current
    {
        get { return _Current; }
        set
        {
            _Current = value;
            Progress = value / _Maximum * 100;
            Invalidate();
        }
    }

    public double Progress
    {
        get { return _Progress; }
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            else if (value > 100)
            {
                value = 100;
            }
            _Progress = Convert.ToInt32(value);
            _Current = value * 0.01 * _Maximum;
            if (Width > 0)
            {
                UpdateProgress();
            }
            Invalidate();
        }
    }

    public Color Color1
    {
        get { return C2; }
        set
        {
            C2 = value;
            UpdateColors();
            Invalidate();
        }
    }

    public Color Color2
    {
        get { return C3; }
        set
        {
            C3 = value;
            UpdateColors();
            Invalidate();
        }
    }

    #endregion

    private readonly SolidBrush B3;
    private readonly Color C1;
    private readonly Pen P1;
    private readonly Pen P2;
    private readonly Pen P3;
    private readonly ColorBlend X;
    private Bitmap B;
    private LinearGradientBrush B1;
    private LinearGradientBrush B2;
    private Graphics G;
    private Rectangle R1;
    private Rectangle R2;

    public MeteProgressBarBright()
    {
        C1 = Color.FromArgb(232, 232, 232); //Background
        P1 = new Pen(Color.FromArgb(70, Color.White), 2F);
        P2 = new Pen(C2);
        P3 = new Pen(Color.FromArgb(100, 100, 100)); //Highlight
        B3 = new SolidBrush(Color.FromArgb(100, Color.White));
        X = new ColorBlend(4);
        X.Colors = new[] { C2, C3, C3, C2 };
        X.Positions = new[] { 0.0F, 0.1F, 0.9F, 1.0F };
        R2 = new Rectangle(2, 2, 2, 2);
        B2 = new LinearGradientBrush(R2, Color.Transparent, Color.Transparent, 180.0F);
        B2.InterpolationColors = X;
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
    }

    public void UpdateColors()
    {
        P2.Color = C2;
        X.Colors = new[] { C2, C3, C3, C2 };
        B2.InterpolationColors = X;
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        R1 = new Rectangle(0, 1, Width, 4);
        B1 = new LinearGradientBrush(R1, Color.FromArgb(24, Color.Black), Color.Transparent, 90.0F);
        UpdateProgress();
        Invalidate();
        base.OnSizeChanged(e);
    }

    public void UpdateProgress()
    {
        if (_Progress == 0)
        {
            return;
        }
        R2 = new Rectangle(2, 2, Convert.ToInt32((Width - 4) * (_Progress * 0.01)), Height - 4);
        B2 = new LinearGradientBrush(R2, Color.Transparent, Color.Transparent, 180.0F);
        B2.InterpolationColors = X;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        B = new Bitmap(Width, Height);
        G = Graphics.FromImage(B);

        G.Clear(C1);

        G.FillRectangle(B1, R1);

        if (_Progress > 0)
        {
            G.FillRectangle(B2, R2);

            G.FillRectangle(B3, 2, 3, R2.Width, 4);
            G.DrawRectangle(P1, 4, 4, R2.Width - 4, Height - 8);

            G.DrawRectangle(P2, 2, 2, R2.Width - 1, Height - 5);
        }

        G.DrawRectangle(P3, 0, 0, Width - 1, Height - 1);

        e.Graphics.DrawImage(B, 0, 0);
        G.Dispose();
        B.Dispose();
    }
}

internal class MeteSeperatorBright : Control
{
    private readonly Color C1;
    private readonly Pen P1;
    private readonly Pen P2;
    private Bitmap B;
    private Graphics G;
    private int I;
    private Orientation _Orientation;

    public MeteSeperatorBright()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        C1 = Color.FromArgb(248, 248, 248); //Background
        P1 = new Pen(Color.FromArgb(230, 230, 230)); //Shadow
        P2 = new Pen(Color.FromArgb(255, 255, 255)); //Highlight
    }

    public Orientation Orientation
    {
        get { return _Orientation; }
        set
        {
            _Orientation = value;
            UpdateOffset();
            Invalidate();
        }
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        UpdateOffset();
        base.OnSizeChanged(e);
    }

    public void UpdateOffset()
    {
        I = Convert.ToInt32(((_Orientation == 0) ? Height / 2 - 1 : Width / 2 - 1));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        B = new Bitmap(Width, Height);
        G = Graphics.FromImage(B);

        G.Clear(C1);

        if (_Orientation == 0)
        {
            G.DrawLine(P1, 0, I, Width, I);
            G.DrawLine(P2, 0, I + 1, Width, I + 1);
        }
        else
        {
            G.DrawLine(P2, I, 0, I, Height);
            G.DrawLine(P1, I + 1, 0, I + 1, Height);
        }

        e.Graphics.DrawImage(B, 0, 0);
        G.Dispose();
        B.Dispose();
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
    }
}

internal class MeteSeperatorDark : Control
{
    private readonly Color C1;
    private readonly Pen P1;
    private readonly Pen P2;
    private Bitmap B;
    private Graphics G;
    private int I;
    private Orientation _Orientation;

    public MeteSeperatorDark()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        C1 = Color.FromArgb(35, 35, 35); //Background
        P1 = new Pen(Color.FromArgb(255, 168, 0)); //Shadow
        P2 = new Pen(Color.FromArgb(100, 100, 100)); //Highlight
    }

    public Orientation Orientation
    {
        get { return _Orientation; }
        set
        {
            _Orientation = value;
            UpdateOffset();
            Invalidate();
        }
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        UpdateOffset();
        base.OnSizeChanged(e);
    }

    public void UpdateOffset()
    {
        I = Convert.ToInt32(((_Orientation == 0) ? Height / 2 - 1 : Width / 2 - 1));
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        B = new Bitmap(Width, Height);
        G = Graphics.FromImage(B);

        G.Clear(C1);

        if (_Orientation == 0)
        {
            G.DrawLine(P1, 0, I, Width, I);
            G.DrawLine(P2, 0, I + 1, Width, I + 1);
        }
        else
        {
            G.DrawLine(P2, I, 0, I, Height);
            G.DrawLine(P1, I + 1, 0, I + 1, Height);
        }

        e.Graphics.DrawImage(B, 0, 0);
        G.Dispose();
        B.Dispose();
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
    }
}

#endregion

#region MeteButtonColor Elemente (Rot, Grün, Blau, Orange, Schwarz, Weiss, Braun, Lila, Pink, Gelb, Dunkelrot, Dunkelgrün, Dunkelblau)

internal class MeteButtonGreen : ThemeControl
{
    public override void PaintHook()
    {
        Font = new Font("Arial", 10);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                var p1 = new Pen(Color.FromArgb(120, 159, 22), 1);
                var x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(157, 209, 57),
                    Color.FromArgb(130, 181, 18), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(190, 232, 109)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0);
                break;
            case State.MouseDown:
                var p2 = new Pen(Color.FromArgb(120, 159, 22), 1);
                var x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(125, 171, 25),
                    Color.FromArgb(142, 192, 40), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(142, 172, 30)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1);
                break;
            case State.MouseOver:
                var p3 = new Pen(Color.FromArgb(120, 159, 22), 1);
                var x3 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(165, 220, 59),
                    Color.FromArgb(137, 191, 18), LinearGradientMode.Vertical);
                G.FillPath(x3, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p3, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(190, 232, 109)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1);
                break;
        }
        Cursor = Cursors.Hand;
    }
}

internal class MeteButtonBlue : ThemeControl
{
    public override void PaintHook()
    {
        Font = new Font("Arial", 10);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                var p = new Pen(Color.FromArgb(34, 112, 171), 1);
                var x = new LinearGradientBrush(ClientRectangle, Color.FromArgb(51, 159, 231),
                    Color.FromArgb(33, 128, 206), LinearGradientMode.Vertical);
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(131, 197, 241)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0);
                break;
            case State.MouseDown:
                var p1 = new Pen(Color.FromArgb(34, 112, 171), 1);
                var x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(196, 37, 37),
                    Color.FromArgb(53, 153, 219), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));

                DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1);
                break;
            case State.MouseOver:
                var p2 = new Pen(Color.FromArgb(34, 112, 171), 1);
                var x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(54, 167, 243),
                    Color.FromArgb(35, 165, 217), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(131, 197, 241)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1);
                break;
        }
        Cursor = Cursors.Hand;
    }
}

internal class MeteButtonRed : ThemeControl
{
    public override void PaintHook()
    {
        Font = new Font("Arial", 10);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                var p = new Pen(Color.FromArgb(171, 34, 34), 1);
                var x = new LinearGradientBrush(ClientRectangle, Color.FromArgb(219, 53, 53),
                    Color.FromArgb(206, 33, 33), LinearGradientMode.Vertical);
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(241, 131, 131)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0);
                break;
            case State.MouseDown:
                var p1 = new Pen(Color.FromArgb(171, 34, 34), 1);
                var x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(196, 37, 37),
                    Color.FromArgb(219, 53, 53), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));

                DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1);
                break;
            case State.MouseOver:
                var p2 = new Pen(Color.FromArgb(171, 34, 34), 1);
                var x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(243, 54, 54),
                    Color.FromArgb(217, 35, 35), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(241, 131, 131)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1);
                break;
        }
        Cursor = Cursors.Hand;
    }
}

internal class MeteButtonOrange : ThemeControl
{
    public override void PaintHook()
    {
        Font = new Font("Arial", 10);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                var p = new Pen(Color.FromArgb(255, 207, 77), 1);
                var x = new LinearGradientBrush(ClientRectangle, Color.FromArgb(255, 223, 135),
                    Color.FromArgb(255, 187, 0), LinearGradientMode.Vertical);
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(252, 232, 174)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(0, 0, 0), 0);
                break;
            case State.MouseDown:
                var p1 = new Pen(Color.FromArgb(235, 197, 57), 1);
                var x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(225, 193, 105),
                    Color.FromArgb(255, 187, 53), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));

                DrawText(HorizontalAlignment.Center, Color.FromArgb(0, 0, 0), 1);
                break;
            case State.MouseOver:
                var p2 = new Pen(Color.FromArgb(235, 220, 34), 1);
                var x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(243, 193, 105),
                    Color.FromArgb(217, 187, 35), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(252, 232, 174)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(0, 0, 0), -1);
                break;
        }
        Cursor = Cursors.Hand;
    }
}

internal class MeteButtonBlack : ThemeControl
{
    public override void PaintHook()
    {
        Font = new Font("Arial", 10);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                var p = new Pen(Color.FromArgb(0, 0, 0), 1);
                var x = new LinearGradientBrush(ClientRectangle, Color.FromArgb(10, 10, 10),
                    Color.FromArgb(50, 50, 50), LinearGradientMode.Vertical);
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(30, 30, 30)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0);
                break;
            case State.MouseDown:
                var p1 = new Pen(Color.FromArgb(0, 0, 0), 1);
                var x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(10, 10, 10),
                    Color.FromArgb(120, 120, 120), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));

                DrawText(HorizontalAlignment.Center, Color.FromArgb(210, 210, 210), 1);
                break;
            case State.MouseOver:
                var p2 = new Pen(Color.FromArgb(0, 0, 0), 1);
                var x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(10, 10, 10),
                    Color.FromArgb(80, 80, 80), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));

                DrawText(HorizontalAlignment.Center, Color.FromArgb(210, 210, 210), 1);
                break;
        }
        Cursor = Cursors.Hand;
    }
}

internal class MeteButtonWhite : ThemeControl
{
    public override void PaintHook()
    {
        Font = new Font("Arial", 10);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                var p = new Pen(Color.FromArgb(255, 255, 255), 1);
                var x = new LinearGradientBrush(ClientRectangle, Color.FromArgb(255, 255, 255),
                    Color.FromArgb(240, 240, 240), LinearGradientMode.Vertical);
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(0, 0, 0), 0);
                break;
            case State.MouseDown:
                var p1 = new Pen(Color.FromArgb(210, 210, 210), 1);
                var x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(200, 200, 200),
                    Color.FromArgb(120, 120, 120), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(0, 0, 0), 0);
                break;
            case State.MouseOver:
                var p2 = new Pen(Color.FromArgb(255, 255, 255), 1);
                var x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(200, 200, 200),
                    Color.FromArgb(170, 170, 170), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(0, 0, 0), 0);
                break;
        }
        Cursor = Cursors.Hand;
    }
}

internal class MeteButtonBrown : ThemeControl
{
    public override void PaintHook()
    {
        Font = new Font("Arial", 10);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                var p = new Pen(Color.FromArgb(154, 50, 50), 1);
                var x = new LinearGradientBrush(ClientRectangle, Color.FromArgb(154, 50, 50),
                    Color.FromArgb(150, 50, 0), LinearGradientMode.Vertical);
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(154, 50, 50)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0);
                break;
            case State.MouseDown:
                var p1 = new Pen(Color.FromArgb(154, 50, 50), 1);
                var x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(150, 50, 0),
                    Color.FromArgb(154, 0, 0), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));

                DrawText(HorizontalAlignment.Center, Color.FromArgb(200, 200, 200), 1);
                break;
            case State.MouseOver:
                var p2 = new Pen(Color.FromArgb(154, 50, 50), 1);
                var x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(150, 50, 0),
                    Color.FromArgb(154, 0, 0), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(154, 50, 50)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1);
                break;
        }
        Cursor = Cursors.Hand;
    }
}

internal class MeteButtonPurple : ThemeControl
{
    public override void PaintHook()
    {
        Font = new Font("Arial", 10);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                var p = new Pen(Color.FromArgb(200, 100, 180), 1);
                var x = new LinearGradientBrush(ClientRectangle, Color.FromArgb(200, 100, 180),
                    Color.FromArgb(170, 0, 120), LinearGradientMode.Vertical);
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(200, 100, 131)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0);
                break;
            case State.MouseDown:
                var p1 = new Pen(Color.FromArgb(180, 80, 160), 1);
                var x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(180, 80, 160),
                    Color.FromArgb(150, 0, 100), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1);
                break;
            case State.MouseOver:
                var p2 = new Pen(Color.FromArgb(200, 100, 180), 1);
                var x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(170, 0, 120),
                    Color.FromArgb(200, 100, 180), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(200, 100, 131)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1);
                break;
        }
        Cursor = Cursors.Hand;
    }
}

internal class MeteButtonPink : ThemeControl
{
    public override void PaintHook()
    {
        Font = new Font("Arial", 10);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                var p = new Pen(Color.FromArgb(255, 0, 130), 1);
                var x = new LinearGradientBrush(ClientRectangle, Color.FromArgb(255, 0, 130),
                    Color.FromArgb(255, 0, 255), LinearGradientMode.Vertical);
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(255, 0, 255)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0);
                break;
            case State.MouseDown:
                var p1 = new Pen(Color.FromArgb(255, 0, 200), 1);
                var x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(255, 0, 130),
                    Color.FromArgb(255, 0, 255), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1);
                break;
            case State.MouseOver:
                var p2 = new Pen(Color.FromArgb(255, 0, 250), 1);
                var x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(255, 0, 255),
                    Color.FromArgb(255, 0, 130), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(255, 0, 255)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1);
                break;
        }
        Cursor = Cursors.Hand;
    }
}

internal class MeteButtonYellow : ThemeControl
{
    public override void PaintHook()
    {
        Font = new Font("Arial", 10);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                var p = new Pen(Color.FromArgb(255, 230, 0), 1);
                var x = new LinearGradientBrush(ClientRectangle, Color.FromArgb(255, 230, 0),
                    Color.FromArgb(255, 180, 0), LinearGradientMode.Vertical);
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(255, 230, 0)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(50, 60, 60), 0);
                break;
            case State.MouseDown:
                var p1 = new Pen(Color.FromArgb(255, 230, 0), 1);
                var x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(255, 230, 0),
                    Color.FromArgb(255, 150, 0), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                DrawText(HorizontalAlignment.Center, Color.FromArgb(0, 0, 0), 1);
                break;
            case State.MouseOver:
                var p2 = new Pen(Color.FromArgb(255, 180, 0), 1);
                var x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(255, 180, 0),
                    Color.FromArgb(255, 230, 0), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(241, 131, 131)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(200, 200, 200), -1);
                break;
        }
        Cursor = Cursors.Hand;
    }
}

internal class MeteButtonDarkRed : ThemeControl
{
    public override void PaintHook()
    {
        Font = new Font("Arial", 10);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                var p = new Pen(Color.FromArgb(200, 0, 0), 1);
                var x = new LinearGradientBrush(ClientRectangle, Color.FromArgb(200, 0, 0),
                    Color.FromArgb(180, 0, 0), LinearGradientMode.Vertical);
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(200, 0, 0)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0);
                break;
            case State.MouseDown:
                var p1 = new Pen(Color.FromArgb(200, 34, 34), 1);
                var x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(100, 0, 0),
                    Color.FromArgb(160, 0, 0), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1);
                break;
            case State.MouseOver:
                var p2 = new Pen(Color.FromArgb(180, 0, 0), 1);
                var x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(180, 0, 0),
                    Color.FromArgb(200, 0, 0), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(180, 0, 0)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1);
                break;
        }
        Cursor = Cursors.Hand;
    }
}

internal class MeteButtonDarkGreen : ThemeControl
{
    public override void PaintHook()
    {
        Font = new Font("Arial", 10);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                var p = new Pen(Color.FromArgb(0, 150, 0), 1);
                var x = new LinearGradientBrush(ClientRectangle, Color.FromArgb(0, 100, 0),
                    Color.FromArgb(0, 70, 50), LinearGradientMode.Vertical);
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(0, 130, 0)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0);
                break;
            case State.MouseDown:
                var p1 = new Pen(Color.FromArgb(0, 100, 0), 1);
                var x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(0, 100, 0),
                    Color.FromArgb(0, 70, 50), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1);
                break;
            case State.MouseOver:
                var p2 = new Pen(Color.FromArgb(0, 100, 0), 1);
                var x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(0, 70, 50),
                    Color.FromArgb(0, 100, 0), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(0, 130, 0)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1);
                break;
        }
        Cursor = Cursors.Hand;
    }
}

internal class MeteButtonDarkBlue : ThemeControl
{
    public override void PaintHook()
    {
        Font = new Font("Arial", 10);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;
        switch (MouseState)
        {
            case State.MouseNone:
                var p = new Pen(Color.FromArgb(0, 0, 100), 1);
                var x = new LinearGradientBrush(ClientRectangle, Color.FromArgb(0, 0, 80),
                    Color.FromArgb(0, 0, 50), LinearGradientMode.Vertical);
                G.FillPath(x, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(0, 0, 110)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), 0);
                break;
            case State.MouseDown:
                var p1 = new Pen(Color.FromArgb(0, 0, 100), 1);
                var x1 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(0, 0, 70),
                    Color.FromArgb(0, 0, 50), LinearGradientMode.Vertical);
                G.FillPath(x1, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p1, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                DrawText(HorizontalAlignment.Center, Color.FromArgb(250, 250, 250), 1);
                break;
            case State.MouseOver:
                var p2 = new Pen(Color.FromArgb(0, 0, 50), 1);
                var x2 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(0, 0, 50),
                    Color.FromArgb(0, 0, 80), LinearGradientMode.Vertical);
                G.FillPath(x2, Draw.RoundRect(ClientRectangle, 4));
                G.DrawPath(p2, Draw.RoundRect(new Rectangle(0, 0, Width - 1, Height - 1), 3));
                G.DrawLine(new Pen(Color.FromArgb(0, 0, 80)), 2, 1, Width - 3, 1);
                DrawText(HorizontalAlignment.Center, Color.FromArgb(240, 240, 240), -1);
                break;
        }
        Cursor = Cursors.Hand;
    }
}

#endregion

#region Special Elemente: Light -> Hell & Dunkel (Light = Hell | Dark = Dunkel)

#region MeteLight

internal abstract class MeteLightContainer : ContainerControl
{
    #region " Initialization "

    protected Bitmap B;
    protected Graphics G;

    public MeteLightContainer()
    {
        SetStyle((ControlStyles)139270, true);
        B = new Bitmap(1, 1);
        G = Graphics.FromImage(B);
    }

    public void AllowTransparent()
    {
        SetStyle(ControlStyles.Opaque, false);
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
    }

    #endregion

    #region " Convienence "

    private LinearGradientBrush _Gradient;
    private bool _NoRounding;
    private Rectangle _Rectangle;

    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    public abstract void PaintHook();

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;
        PaintHook();
        e.Graphics.DrawImage(B, 0, 0);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        if (!(Width == 0) && !(Height == 0))
        {
            B = new Bitmap(Width, Height);
            G = Graphics.FromImage(B);
            Invalidate();
        }
        base.OnSizeChanged(e);
    }

    protected void DrawCorners(Color c, Rectangle rect)
    {
        if (_NoRounding)
            return;
        B.SetPixel(rect.X, rect.Y, c);
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c);
        B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c);
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c);
    }

    protected void DrawBorders(Pen p1, Pen p2, Rectangle rect)
    {
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
    }

    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        _Rectangle = new Rectangle(x, y, width, height);
        _Gradient = new LinearGradientBrush(_Rectangle, c1, c2, angle);
        G.FillRectangle(_Gradient, _Rectangle);
    }

    #endregion
}

internal class MeteLightGrpPnlBox : ThemeContainerControl
{
    public MeteLightGrpPnlBox()
    {
        AllowTransparent();
    }

    public override void PaintHook()
    {
        Font = new Font("Tahoma", 10);
        ForeColor = Color.FromArgb(40, 40, 40);
        G.SmoothingMode = SmoothingMode.AntiAlias;
        G.Clear(Color.FromArgb(245, 245, 245));
        G.FillRectangle(new SolidBrush(Color.FromArgb(231, 231, 231)), new Rectangle(0, 0, Width, 30));
        G.DrawLine(new Pen(Color.FromArgb(233, 238, 240)), 1, 1, Width - 2, 1);
        G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, Height - 1);
        G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, 30);
        G.DrawString(Text, Font, new SolidBrush(ForeColor), 7, 6);
    }
}

internal class MeteLightPanelBox : ThemeContainerControl
{
    public MeteLightPanelBox()
    {
        AllowTransparent();
    }

    public override void PaintHook()
    {
        Font = new Font("Tahoma", 10);
        ForeColor = Color.FromArgb(40, 40, 40);
        G.SmoothingMode = SmoothingMode.AntiAlias;
        G.FillRectangle(new SolidBrush(Color.FromArgb(235, 235, 235)), new Rectangle(2, 0, Width, Height));
        G.FillRectangle(new SolidBrush(Color.FromArgb(249, 249, 249)), new Rectangle(1, 0, Width - 3, Height - 4));
        G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 2, Height - 3);
    }
}

internal class MeteLightGrpDrpDwn : ThemeContainerControl
{
    private int X;
    private bool _Checked;
    private Size _OpenedSize;
    private int y;

    public MeteLightGrpDrpDwn()
    {
        AllowTransparent();
        Size = new Size(90, 30);
        MinimumSize = new Size(5, 30);
        _Checked = true;
        Resize += MeteLightGrpDrpDwn_Resize;
        MouseDown += MeteLightGrpDrpDwn_MouseDown;
    }

    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }

    public Size OpenSize
    {
        get { return _OpenedSize; }
        set
        {
            _OpenedSize = value;
            Invalidate();
        }
    }

    public override void PaintHook()
    {
        Font = new Font("Tahoma", 10);
        ForeColor = Color.FromArgb(40, 40, 40);
        if (_Checked)
        {
            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.Clear(Color.FromArgb(245, 245, 245));
            G.FillRectangle(new SolidBrush(Color.FromArgb(231, 231, 231)), new Rectangle(0, 0, Width, 30));
            G.DrawLine(new Pen(Color.FromArgb(233, 238, 240)), 1, 1, Width - 2, 1);
            G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, Height - 1);
            G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, 30);
            Size = _OpenedSize;
            G.DrawString("t", new Font("Marlett", 12), new SolidBrush(ForeColor), Width - 25, 5);
        }
        else
        {
            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.Clear(Color.FromArgb(245, 245, 245));
            G.FillRectangle(new SolidBrush(Color.FromArgb(231, 231, 231)), new Rectangle(0, 0, Width, 30));
            G.DrawLine(new Pen(Color.FromArgb(231, 236, 238)), 1, 1, Width - 2, 1);
            G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, Height - 1);
            G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, 30);
            Size = new Size(Width, 30);
            G.DrawString("u", new Font("Marlett", 12), new SolidBrush(ForeColor), Width - 25, 5);
        }
        G.DrawString(Text, Font, new SolidBrush(ForeColor), 7, 6);
    }

    private void MeteLightGrpDrpDwn_Resize(object sender, EventArgs e)
    {
        if (_Checked)
        {
            _OpenedSize = Size;
        }
    }


    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.X;
        y = e.Y;
        Invalidate();
    }


    private void MeteLightGrpDrpDwn_MouseDown(object sender, MouseEventArgs e)
    {
        if (X >= Width - 22)
        {
            if (y <= 30)
            {
                switch (Checked)
                {
                    case true:
                        Checked = false;
                        break;
                    case false:
                        Checked = true;
                        break;
                }
            }
        }
    }
}

internal class MeteLightGrpPanelBox : ThemeContainerControl
{
    public MeteLightGrpPanelBox()
    {
        AllowTransparent();
    }

    public override void PaintHook()
    {
        Font = new Font("Tahoma", 10);
        ForeColor = Color.FromArgb(40, 40, 40);
        G.SmoothingMode = SmoothingMode.AntiAlias;
        G.Clear(Color.FromArgb(245, 245, 245));
        G.FillRectangle(new SolidBrush(Color.FromArgb(231, 231, 231)), new Rectangle(0, 0, Width, 30));
        G.DrawLine(new Pen(Color.FromArgb(233, 238, 240)), 1, 1, Width - 2, 1);
        G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, Height - 1);
        G.DrawRectangle(new Pen(Color.FromArgb(214, 214, 214)), 0, 0, Width - 1, 30);
        G.DrawString(Text, Font, new SolidBrush(ForeColor), 7, 6);
    }
}

#endregion

#region MeteDark

internal abstract class MeteDarkContainer : ContainerControl
{
    #region " Initialization "

    protected Bitmap B;
    protected Graphics G;

    public MeteDarkContainer()
    {
        SetStyle((ControlStyles)139270, true);
        B = new Bitmap(1, 1);
        G = Graphics.FromImage(B);
    }

    public void AllowTransparent()
    {
        SetStyle(ControlStyles.Opaque, false);
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
    }

    #endregion

    #region " Convienence "

    private LinearGradientBrush _Gradient;
    private bool _NoRounding;
    private Rectangle _Rectangle;

    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    public abstract void PaintHook();

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;
        PaintHook();
        e.Graphics.DrawImage(B, 0, 0);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        if (!(Width == 0) && !(Height == 0))
        {
            B = new Bitmap(Width, Height);
            G = Graphics.FromImage(B);
            Invalidate();
        }
        base.OnSizeChanged(e);
    }

    protected void DrawCorners(Color c, Rectangle rect)
    {
        if (_NoRounding)
            return;
        B.SetPixel(rect.X, rect.Y, c);
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c);
        B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c);
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c);
    }

    protected void DrawBorders(Pen p1, Pen p2, Rectangle rect)
    {
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3);
    }

    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        _Rectangle = new Rectangle(x, y, width, height);
        _Gradient = new LinearGradientBrush(_Rectangle, c1, c2, angle);
        G.FillRectangle(_Gradient, _Rectangle);
    }

    #endregion
}

internal class MeteDarkGrpDrpDwn : ThemeContainerControl
{
    private int X;
    private bool _Checked;
    private Size _OpenedSize;
    private int y;

    public MeteDarkGrpDrpDwn()
    {
        AllowTransparent();
        Size = new Size(90, 30);
        MinimumSize = new Size(5, 30);
        _Checked = true;
        Resize += MeteDarkGrpDrpDwn_Resize;
        MouseDown += MeteDarkGrpDrpDwn_MouseDown;
    }

    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }

    public Size OpenSize
    {
        get { return _OpenedSize; }
        set
        {
            _OpenedSize = value;
            Invalidate();
        }
    }

    public override void PaintHook()
    {
        Font = new Font("Tahoma", 10);
        ForeColor = Color.FromArgb(140, 140, 140);
        if (_Checked)
        {
            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.Clear(Color.FromArgb(10, 10, 10));
            G.FillRectangle(new SolidBrush(Color.FromArgb(22, 22, 22)), new Rectangle(0, 0, Width, 30));
            G.DrawLine(new Pen(Color.FromArgb(17, 17, 15)), 1, 1, Width - 2, 1);
            G.DrawRectangle(new Pen(Color.FromArgb(41, 41, 41)), 0, 0, Width - 1, Height - 1);
            G.DrawRectangle(new Pen(Color.FromArgb(41, 41, 41)), 0, 0, Width - 1, 30);
            Size = _OpenedSize;
            G.DrawString("t", new Font("Marlett", 12), new SolidBrush(ForeColor), Width - 25, 5);
        }
        else
        {
            G.SmoothingMode = SmoothingMode.AntiAlias;
            G.Clear(Color.FromArgb(10, 10, 10));
            G.FillRectangle(new SolidBrush(Color.FromArgb(22, 22, 22)), new Rectangle(0, 0, Width, 30));
            G.DrawLine(new Pen(Color.FromArgb(14, 14, 14)), 1, 1, Width - 2, 1);
            G.DrawRectangle(new Pen(Color.FromArgb(41, 41, 41)), 0, 0, Width - 1, Height - 1);
            G.DrawRectangle(new Pen(Color.FromArgb(41, 41, 41)), 0, 0, Width - 1, 30);
            Size = new Size(Width, 30);
            G.DrawString("u", new Font("Marlett", 12), new SolidBrush(ForeColor), Width - 25, 5);
        }
        G.DrawString(Text, Font, new SolidBrush(ForeColor), 7, 6);
    }

    private void MeteDarkGrpDrpDwn_Resize(object sender, EventArgs e)
    {
        if (_Checked)
        {
            _OpenedSize = Size;
        }
    }


    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.X;
        y = e.Y;
        Invalidate();
    }


    private void MeteDarkGrpDrpDwn_MouseDown(object sender, MouseEventArgs e)
    {
        if (X >= Width - 22)
        {
            if (y <= 30)
            {
                switch (Checked)
                {
                    case true:
                        Checked = false;
                        break;
                    case false:
                        Checked = true;
                        break;
                }
            }
        }
    }
}

internal class MeteDarkGrpPnlBox : ThemeContainerControl
{
    public MeteDarkGrpPnlBox()
    {
        AllowTransparent();
    }

    public override void PaintHook()
    {
        Font = new Font("Tahoma", 10);
        ForeColor = Color.FromArgb(140, 140, 140);
        G.SmoothingMode = SmoothingMode.AntiAlias;
        G.Clear(Color.FromArgb(10, 10, 10));
        G.FillRectangle(new SolidBrush(Color.FromArgb(22, 22, 22)), new Rectangle(0, 0, Width, 30));
        G.DrawLine(new Pen(Color.FromArgb(17, 17, 15)), 1, 1, Width - 2, 1);
        G.DrawRectangle(new Pen(Color.FromArgb(41, 41, 41)), 0, 0, Width - 1, Height - 1);
        G.DrawRectangle(new Pen(Color.FromArgb(41, 41, 41)), 0, 0, Width - 1, 30);
        G.DrawString(Text, Font, new SolidBrush(ForeColor), 7, 6);
    }
}

#endregion

#region Custom Unsorted

public class _okLabel : Control
{
    public _okLabel()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(205, 205, 205);
        DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        var ClientRectangle = new Rectangle(0, 0, Width - 1, Height - 1);
        base.OnPaint(e);
        G.Clear(BackColor);
        var drawFont = new Font("Tahoma", 9, FontStyle.Bold);
        var format = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
        G.CompositingQuality = CompositingQuality.HighQuality;
        G.SmoothingMode = SmoothingMode.HighQuality;
        G.DrawString(Text, drawFont, new SolidBrush(Color.FromArgb(5, 5, 5)), new Rectangle(1, 0, Width - 1, Height - 1),
            format);
        G.DrawString(Text, drawFont, new SolidBrush(Color.GreenYellow), new Rectangle(0, -1, Width - 1, Height - 1),
            format);
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}

public class _warningLabel : Control
{
    public _warningLabel()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(205, 205, 205);
        DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        var ClientRectangle = new Rectangle(0, 0, Width - 1, Height - 1);
        base.OnPaint(e);
        G.Clear(BackColor);
        var drawFont = new Font("Tahoma", 9, FontStyle.Bold);
        var format = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
        G.CompositingQuality = CompositingQuality.HighQuality;
        G.SmoothingMode = SmoothingMode.HighQuality;
        G.DrawString(Text, drawFont, new SolidBrush(Color.FromArgb(5, 5, 5)), new Rectangle(1, 0, Width - 1, Height - 1),
            format);
        G.DrawString(Text, drawFont, new SolidBrush(Color.Red), new Rectangle(0, -1, Width - 1, Height - 1), format);
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}

#endregion

#endregion

#region fusion

internal class Pigment
{
    public Pigment()
    {
    }

    public Pigment(string n, Color v)
    {
        Name = n;
        Value = v;
    }

    public Pigment(string n, byte a, byte r, byte g, byte b)
    {
        Name = n;
        Value = Color.FromArgb(a, r, g, b);
    }

    public Pigment(string n, byte r, byte g, byte b)
    {
        Name = n;
        Value = Color.FromArgb(r, g, b);
    }

    public string Name { get; set; }
    public Color Value { get; set; }
}

internal class FTheme : ContainerControl
{
    public enum Direction
    {
        NONE = 0,
        LEFT = 10,
        RIGHT = 11,
        TOP = 12,
        TOPLEFT = 13,
        TOPRIGHT = 14,
        BOTTOM = 15,
        BOTTOMLEFT = 16,
        BOTTOMRIGHT = 17
    }

    private const byte Count = 8;
    private Bitmap B;

    private SolidBrush B1;
    private SolidBrush B2;
    private LinearGradientBrush B3;
    private LinearGradientBrush B4;

    private Pigment[] C =
    {
        new Pigment("Border", Color.Black),
        new Pigment("Frame", 47, 47, 50),
        new Pigment("Border Highlight", 15, 255, 255, 255),
        new Pigment("Side Highlight", 6, 255, 255, 255),
        new Pigment("Shine", 20, 255, 255, 255),
        new Pigment("Shadow", 38, 38, 40),
        new Pigment("Backcolor", 247, 247, 251),
        new Pigment("Transparency", Color.Fuchsia)
    };

    private ColorBlend CB;
    private Direction Current;
    private Graphics G;
    private Pen P1;
    private Pen P2;
    private Pen P3;
    private Rectangle R1;
    private Rectangle R2;

    public FTheme()
    {
        SetStyle((ControlStyles)8198, true);
        Pigment[] C =
        {
            new Pigment("Border", Color.Black),
            new Pigment("Frame", 47, 47, 50),
            new Pigment("Border Highlight", 15, 255, 255, 255),
            new Pigment("Side Highlight", 6, 255, 255, 255),
            new Pigment("Shine", 20, 255, 255, 255),
            new Pigment("Shadow", 38, 38, 40),
            new Pigment("Backcolor", 247, 247, 251),
            new Pigment("Transparency", Color.Fuchsia)
        };
    }

    public bool Resizeable { get; set; }

    public Pigment[] Colors
    {
        get { return C; }
        set
        {
            if (value.Length != Count)
                throw new IndexOutOfRangeException();

            P1 = new Pen(value[0].Value);
            P2 = new Pen(value[2].Value);

            B1 = new SolidBrush(value[6].Value);
            B2 = new SolidBrush(value[7].Value);

            if (Parent != null)
            {
                Parent.BackColor = value[6].Value;
                if (Parent is Form)
                    ((Form)Parent).TransparencyKey = value[7].Value;
            }

            CB = new ColorBlend();
            CB.Colors = new[]
            {
                Color.Transparent,
                value[4].Value,
                Color.Transparent
            };
            CB.Positions = new[]
            {
                0,
                (float) 0.5,
                1
            };

            C = value;

            Invalidate();
        }
    }

    private Rectangle Drag
    {
        get { return new Rectangle(7, 7, Width - 14, 35); }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        Dock = DockStyle.Fill;
        if (Parent is Form)
            ((Form)Parent).FormBorderStyle = FormBorderStyle.None;
        Colors = C;
        base.OnHandleCreated(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        B = new Bitmap(Width, Height);
        G = Graphics.FromImage(B);

        G.Clear(C[1].Value);

        G.DrawRectangle(P2, new Rectangle(1, 1, Width - 3, Height - 3));
        G.DrawRectangle(P2, new Rectangle(12, 40, Width - 24, Height - 52));

        R1 = new Rectangle(1, 0, 15, Height);
        B3 = new LinearGradientBrush(R1, C[3].Value, Color.Transparent, 90f);
        G.FillRectangle(B3, R1);
        G.FillRectangle(B3, new Rectangle(Width - 16, 0, 15, Height));

        G.FillRectangle(B1, new Rectangle(13, 41, Width - 26, Height - 54));

        R2 = new Rectangle(0, 2, Width, 2);
        B4 = new LinearGradientBrush(R2, Color.Empty, Color.Empty, 0F);
        B4.InterpolationColors = CB;
        G.FillRectangle(B4, R2);

        G.DrawRectangle(P1, new Rectangle(13, 41, Width - 26, Height - 54));
        G.DrawRectangle(P1, new Rectangle(0, 0, Width - 1, Height - 1));

        G.FillRectangle(B2, new Rectangle(0, 0, 2, 2));
        G.FillRectangle(B2, new Rectangle(Width - 2, 0, 2, 2));
        G.FillRectangle(B2, new Rectangle(Width - 2, Height - 2, 2, 2));
        G.FillRectangle(B2, new Rectangle(0, Height - 2, 2, 2));

        B.SetPixel(1, 1, Color.Black);
        B.SetPixel(Width - 2, 1, Color.Black);
        B.SetPixel(Width - 2, Height - 2, Color.Black);
        B.SetPixel(1, Height - 2, Color.Black);

        e.Graphics.DrawImage(B, 0, 0);
        B3.Dispose();
        B4.Dispose();
        G.Dispose();
        B.Dispose();
    }

    public void SetCurrent()
    {
        Point T = PointToClient(MousePosition);
        if (T.X < 7 & T.Y < 7)
        {
            Current = Direction.TOPLEFT;
            Cursor = Cursors.SizeNWSE;
        }
        else if (T.X < 7 & T.Y > Height - 7)
        {
            Current = Direction.BOTTOMLEFT;
            Cursor = Cursors.SizeNESW;
        }
        else if (T.X > Width - 7 & T.Y > Height - 7)
        {
            Current = Direction.BOTTOMRIGHT;
            Cursor = Cursors.SizeNWSE;
        }
        else if (T.X > Width - 7 & T.Y < 7)
        {
            Current = Direction.TOPRIGHT;
            Cursor = Cursors.SizeNESW;
        }
        else if (T.X < 7)
        {
            Current = Direction.LEFT;
            Cursor = Cursors.SizeWE;
        }
        else if (T.X > Width - 7)
        {
            Current = Direction.RIGHT;
            Cursor = Cursors.SizeWE;
        }
        else if (T.Y < 7)
        {
            Current = Direction.TOP;
            Cursor = Cursors.SizeNS;
        }
        else if (T.Y > Height - 7)
        {
            Current = Direction.BOTTOM;
            Cursor = Cursors.SizeNS;
        }
        else
        {
            Current = Direction.NONE;
            Cursor = Cursors.Default;
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            if (Parent is Form)
            {
                if (((Form)Parent).WindowState == FormWindowState.Maximized)
                    return;
            }
            if (Drag.Contains(e.Location))
            {
                Capture = false;
                var Val = new IntPtr(2);
                IntPtr NULL = IntPtr.Zero;
                Message msg = Message.Create(Parent.Handle, 161, Val, NULL);
                DefWndProc(ref msg);
            }
            else
            {
                if (Current != Direction.NONE & Resizeable)
                {
                    Capture = false;
                    var Val = new IntPtr(Convert.ToInt32(Current));
                    IntPtr NULL = IntPtr.Zero;
                    Message msg = Message.Create(Parent.Handle, 161, Val, NULL);
                    DefWndProc(ref msg);
                }
            }
        }
        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (Resizeable)
            SetCurrent();
        base.OnMouseMove(e);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        Invalidate();
        base.OnSizeChanged(e);
    }
}

internal class FButton : Control
{
    private const byte Count = 7;
    private Bitmap B;
    private SolidBrush B1;
    private SolidBrush B2;
    private LinearGradientBrush B3;
    private Pigment[] C;
    private bool Down;
    private Graphics G;
    private Pen P1;
    private Pen P2;
    private Point PT;
    private Size SZ;
    private bool Shadow_ = true;

    public FButton()
    {
        SetStyle((ControlStyles)8198, true);
        Colors = new[]
        {
            new Pigment("Border", 254, 133, 0),
            new Pigment("Backcolor", 247, 247, 251),
            new Pigment("Highlight", 255, 197, 19),
            new Pigment("Gradient1", 255, 175, 12),
            new Pigment("Gradient2", 255, 127, 1),
            new Pigment("Text Color", Color.White),
            new Pigment("Text Shadow", 30, 0, 0, 0)
        };
        Font = new Font("Verdana", 8);
    }

    public bool Shadow
    {
        get { return Shadow_; }
        set
        {
            Shadow_ = value;
            Invalidate();
        }
    }

    public Pigment[] Colors
    {
        get { return C; }
        set
        {
            if (value.Length != Count)
                throw new IndexOutOfRangeException();

            P1 = new Pen(value[0].Value);
            P2 = new Pen(value[2].Value);

            B1 = new SolidBrush(value[6].Value);
            B2 = new SolidBrush(value[5].Value);

            C = value;
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        B = new Bitmap(Width, Height);
        G = Graphics.FromImage(B);

        if (Down)
        {
            B3 = new LinearGradientBrush(ClientRectangle, C[4].Value, C[3].Value, 90f);
        }
        else
        {
            B3 = new LinearGradientBrush(ClientRectangle, C[3].Value, C[4].Value, 90f);
        }
        G.FillRectangle(B3, ClientRectangle);

        if (!string.IsNullOrEmpty(Text))
        {
            SZ = G.MeasureString(Text, Font).ToSize();
            PT = new Point(Convert.ToInt32(Width / 2 - SZ.Width / 2), Convert.ToInt32(Height / 2 - SZ.Height / 2));
            if (Shadow_)
                G.DrawString(Text, Font, B1, PT.X + 1, PT.Y + 1);
            G.DrawString(Text, Font, B2, PT);
        }

        G.DrawRectangle(P1, new Rectangle(0, 0, Width - 1, Height - 1));
        G.DrawRectangle(P2, new Rectangle(1, 1, Width - 3, Height - 3));

        B.SetPixel(0, 0, C[1].Value);
        B.SetPixel(Width - 1, 0, C[1].Value);
        B.SetPixel(Width - 1, Height - 1, C[1].Value);
        B.SetPixel(0, Height - 1, C[1].Value);

        e.Graphics.DrawImage(B, 0, 0);
        B3.Dispose();
        G.Dispose();
        B.Dispose();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            Down = true;
            Invalidate();
        }
        base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        Down = false;
        Invalidate();
        base.OnMouseUp(e);
    }
}

internal class FProgressBar : Control
{
    private const byte Count = 6;
    private Bitmap B;
    private SolidBrush B1;
    private LinearGradientBrush B2;
    private Pigment[] C;

    private ColorBlend CB;
    private Graphics G;
    private Pen P1;
    private Pen P2;
    private double _Current;
    private double _Maximum = 100;
    private double _Progress;

    public FProgressBar()
    {
        SetStyle((ControlStyles)8198, true);
        Colors = new[]
        {
            new Pigment("Border", 214, 214, 216),
            new Pigment("Backcolor1", 247, 247, 251),
            new Pigment("Backcolor2", 239, 239, 242),
            new Pigment("Highlight", 100, 255, 255, 255),
            new Pigment("Forecolor", 224, 224, 224),
            new Pigment("Gloss", 130, 255, 255, 255)
        };
    }

    public double Maximum
    {
        get { return _Maximum; }
        set
        {
            _Maximum = value;
            Progress = _Current / value * 100;
        }
    }

    public double Current
    {
        get { return _Current; }
        set { Progress = value / _Maximum * 100; }
    }

    public double Progress
    {
        get { return _Progress; }
        set
        {
            if (value < 0)
                value = 0;
            else if (value > 100)
                value = 100;
            _Progress = value;
            _Current = value * 0.01 * _Maximum;
            Invalidate();
        }
    }

    public Pigment[] Colors
    {
        get { return C; }
        set
        {
            if (value.Length != Count)
                throw new IndexOutOfRangeException();

            P1 = new Pen(value[0].Value);
            P2 = new Pen(value[3].Value);

            B1 = new SolidBrush(value[4].Value);

            CB = new ColorBlend();
            CB.Colors = new[]
            {
                value[5].Value,
                Color.Transparent,
                Color.Transparent
            };
            CB.Positions = new[]
            {
                0,
                0.3F,
                1
            };

            C = value;
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        B = new Bitmap(Width, Height);
        G = Graphics.FromImage(B);

        G.Clear(C[2].Value);

        G.FillRectangle(B1, new Rectangle(1, 1, Convert.ToInt32((Width * _Progress * 0.01) - 2), Height - 2));

        B2 = new LinearGradientBrush(ClientRectangle, Color.Empty, Color.Empty, 90f);
        B2.InterpolationColors = CB;
        G.FillRectangle(B2, ClientRectangle);

        G.DrawRectangle(P1, new Rectangle(0, 0, Width - 1, Height - 1));
        G.DrawRectangle(P2, new Rectangle(1, 1, Width - 3, Height - 3));

        B.SetPixel(0, 0, C[1].Value);
        B.SetPixel(Width - 1, 0, C[1].Value);
        B.SetPixel(Width - 1, Height - 1, C[1].Value);
        B.SetPixel(0, Height - 1, C[1].Value);

        e.Graphics.DrawImage(B, 0, 0);
        B2.Dispose();
        G.Dispose();
        B.Dispose();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        Invalidate();
        base.OnSizeChanged(e);
    }
}

#endregion

#region origin

internal abstract class ThemeContainer154 : ContainerControl
{
    #region " Initialization "

    protected Bitmap B;
    private bool DoneCreation;
    protected Graphics G;

    public ThemeContainer154()
    {
        SetStyle((ControlStyles)139270, true);

        _ImageSize = Size.Empty;
        Font = new Font("Verdana", 8);

        MeasureBitmap = new Bitmap(1, 1);
        MeasureGraphics = Graphics.FromImage(MeasureBitmap);

        DrawRadialPath = new GraphicsPath();

        InvalidateCustimization();
    }

    protected override sealed void OnHandleCreated(EventArgs e)
    {
        if (DoneCreation)
            InitializeMessages();

        InvalidateCustimization();
        ColorHook();

        if (!(_LockWidth == 0))
            Width = _LockWidth;
        if (!(_LockHeight == 0))
            Height = _LockHeight;
        if (!_ControlMode)
            base.Dock = DockStyle.Fill;

        Transparent = _Transparent;
        if (_Transparent && _BackColor)
            BackColor = Color.Transparent;

        base.OnHandleCreated(e);
    }

    protected override sealed void OnParentChanged(EventArgs e)
    {
        base.OnParentChanged(e);

        if (Parent == null)
            return;
        _IsParentForm = Parent is Form;

        if (!_ControlMode)
        {
            InitializeMessages();

            if (_IsParentForm)
            {
                ParentForm.FormBorderStyle = _BorderStyle;
                ParentForm.TransparencyKey = _TransparencyKey;

                if (!DesignMode)
                {
                    ParentForm.Shown += FormShown;
                }
            }

            Parent.BackColor = BackColor;
        }

        OnCreation();
        DoneCreation = true;
        InvalidateTimer();
    }

    #endregion

    private bool HasShown;

    private void DoAnimation(bool i)
    {
        OnAnimation();
        if (i)
            Invalidate();
    }

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;

        if (_Transparent && _ControlMode)
        {
            PaintHook();
            e.Graphics.DrawImage(B, 0, 0);
        }
        else
        {
            G = e.Graphics;
            PaintHook();
        }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        ThemeShare.RemoveAnimationCallback(DoAnimation);
        base.OnHandleDestroyed(e);
    }

    private void FormShown(object sender, EventArgs e)
    {
        if (_ControlMode || HasShown)
            return;

        if (_StartPosition == FormStartPosition.CenterParent || _StartPosition == FormStartPosition.CenterScreen)
        {
            Rectangle SB = Screen.PrimaryScreen.Bounds;
            Rectangle CB = ParentForm.Bounds;
            ParentForm.Location = new Point(SB.Width / 2 - CB.Width / 2, SB.Height / 2 - CB.Width / 2);
        }

        HasShown = true;
    }

    #region " Size Handling "

    private Rectangle Frame;

    protected override sealed void OnSizeChanged(EventArgs e)
    {
        if (_Movable && !_ControlMode)
        {
            Frame = new Rectangle(7, 7, Width - 14, _Header - 7);
        }

        InvalidateBitmap();
        Invalidate();

        base.OnSizeChanged(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        if (!(_LockWidth == 0))
            width = _LockWidth;
        if (!(_LockHeight == 0))
            height = _LockHeight;
        base.SetBoundsCore(x, y, width, height, specified);
    }

    #endregion

    #region " State Handling "

    private readonly Message[] Messages = new Message[9];
    private bool B1;
    private bool B2;
    private bool B3;
    private bool B4;
    private int Current;
    private Point GetIndexPoint;
    private int Previous;
    protected MouseState State;
    private bool WM_LMBUTTONDOWN;

    private void SetState(MouseState current)
    {
        State = current;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized))
        {
            if (_Sizable && !_ControlMode)
                InvalidateMouse();
        }

        base.OnMouseMove(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        if (Enabled)
            SetState(MouseState.None);
        else
            SetState(MouseState.Block);
        base.OnEnabledChanged(e);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        SetState(MouseState.Over);
        base.OnMouseEnter(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        SetState(MouseState.Over);
        base.OnMouseUp(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        SetState(MouseState.None);

        if (GetChildAtPoint(PointToClient(MousePosition)) != null)
        {
            if (_Sizable && !_ControlMode)
            {
                Cursor = Cursors.Default;
                Previous = 0;
            }
        }

        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            SetState(MouseState.Down);

        if (!(_IsParentForm && ParentForm.WindowState == FormWindowState.Maximized || _ControlMode))
        {
            if (_Movable && Frame.Contains(e.Location))
            {
                Capture = false;
                WM_LMBUTTONDOWN = true;
                DefWndProc(ref Messages[0]);
            }
            else if (_Sizable && !(Previous == 0))
            {
                Capture = false;
                WM_LMBUTTONDOWN = true;
                DefWndProc(ref Messages[Previous]);
            }
        }

        base.OnMouseDown(e);
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        if (WM_LMBUTTONDOWN && m.Msg == 513)
        {
            WM_LMBUTTONDOWN = false;

            SetState(MouseState.Over);
            if (!_SmartBounds)
                return;

            if (IsParentMdi)
            {
                CorrectBounds(new Rectangle(Point.Empty, Parent.Parent.Size));
            }
            else
            {
                CorrectBounds(Screen.FromControl(Parent).WorkingArea);
            }
        }
    }

    private int GetIndex()
    {
        GetIndexPoint = PointToClient(MousePosition);
        B1 = GetIndexPoint.X < 7;
        B2 = GetIndexPoint.X > Width - 7;
        B3 = GetIndexPoint.Y < 7;
        B4 = GetIndexPoint.Y > Height - 7;

        if (B1 && B3)
            return 4;
        if (B1 && B4)
            return 7;
        if (B2 && B3)
            return 5;
        if (B2 && B4)
            return 8;
        if (B1)
            return 1;
        if (B2)
            return 2;
        if (B3)
            return 3;
        if (B4)
            return 6;
        return 0;
    }

    private void InvalidateMouse()
    {
        Current = GetIndex();
        if (Current == Previous)
            return;

        Previous = Current;
        switch (Previous)
        {
            case 0:
                Cursor = Cursors.Default;
                break;
            case 1:
            case 2:
                Cursor = Cursors.SizeWE;
                break;
            case 3:
            case 6:
                Cursor = Cursors.SizeNS;
                break;
            case 4:
            case 8:
                Cursor = Cursors.SizeNWSE;
                break;
            case 5:
            case 7:
                Cursor = Cursors.SizeNESW;
                break;
        }
    }

    private void InitializeMessages()
    {
        Messages[0] = Message.Create(Parent.Handle, 161, new IntPtr(2), IntPtr.Zero);
        for (int I = 1; I <= 8; I++)
        {
            Messages[I] = Message.Create(Parent.Handle, 161, new IntPtr(I + 9), IntPtr.Zero);
        }
    }

    private void CorrectBounds(Rectangle bounds)
    {
        if (Parent.Width > bounds.Width)
            Parent.Width = bounds.Width;
        if (Parent.Height > bounds.Height)
            Parent.Height = bounds.Height;

        int X = Parent.Location.X;
        int Y = Parent.Location.Y;

        if (X < bounds.X)
            X = bounds.X;
        if (Y < bounds.Y)
            Y = bounds.Y;

        int Width = bounds.X + bounds.Width;
        int Height = bounds.Y + bounds.Height;

        if (X + Parent.Width > Width)
            X = Width - Parent.Width;
        if (Y + Parent.Height > Height)
            Y = Height - Parent.Height;

        Parent.Location = new Point(X, Y);
    }

    #endregion

    #region " Base Properties "

    private bool _BackColor;

    public override DockStyle Dock
    {
        get { return base.Dock; }
        set
        {
            if (!_ControlMode)
                return;
            base.Dock = value;
        }
    }

    [Category("Misc")]
    public override Color BackColor
    {
        get { return base.BackColor; }
        set
        {
            if (value == base.BackColor)
                return;

            if (!IsHandleCreated && _ControlMode && value == Color.Transparent)
            {
                _BackColor = true;
                return;
            }

            base.BackColor = value;
            if (Parent != null)
            {
                if (!_ControlMode)
                    Parent.BackColor = value;
                ColorHook();
            }
        }
    }

    public override Size MinimumSize
    {
        get { return base.MinimumSize; }
        set
        {
            base.MinimumSize = value;
            if (Parent != null)
                Parent.MinimumSize = value;
        }
    }

    public override Size MaximumSize
    {
        get { return base.MaximumSize; }
        set
        {
            base.MaximumSize = value;
            if (Parent != null)
                Parent.MaximumSize = value;
        }
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            Invalidate();
        }
    }

    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            Invalidate();
        }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color ForeColor
    {
        get { return Color.Empty; }
        set { }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Image BackgroundImage
    {
        get { return null; }
        set { }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ImageLayout BackgroundImageLayout
    {
        get { return ImageLayout.None; }
        set { }
    }

    #endregion

    #region " Public Properties "

    private readonly Dictionary<string, Color> Items = new Dictionary<string, Color>();
    private FormBorderStyle _BorderStyle;
    private string _Customization;
    private Image _Image;
    private bool _Movable = true;
    private bool _NoRounding;
    private bool _Sizable = true;
    private bool _SmartBounds = true;
    private FormStartPosition _StartPosition;
    private Color _TransparencyKey;
    private bool _Transparent;

    public bool SmartBounds
    {
        get { return _SmartBounds; }
        set { _SmartBounds = value; }
    }

    public bool Movable
    {
        get { return _Movable; }
        set { _Movable = value; }
    }

    public bool Sizable
    {
        get { return _Sizable; }
        set { _Sizable = value; }
    }

    public Color TransparencyKey
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.TransparencyKey;
            return _TransparencyKey;
        }
        set
        {
            if (value == _TransparencyKey)
                return;
            _TransparencyKey = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.TransparencyKey = value;
                ColorHook();
            }
        }
    }

    public FormBorderStyle BorderStyle
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.FormBorderStyle;
            return _BorderStyle;
        }
        set
        {
            _BorderStyle = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.FormBorderStyle = value;

                if (!(value == FormBorderStyle.None))
                {
                    Movable = false;
                    Sizable = false;
                }
            }
        }
    }

    public FormStartPosition StartPosition
    {
        get
        {
            if (_IsParentForm && !_ControlMode)
                return ParentForm.StartPosition;
            return _StartPosition;
        }
        set
        {
            _StartPosition = value;

            if (_IsParentForm && !_ControlMode)
            {
                ParentForm.StartPosition = value;
            }
        }
    }

    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    public Image Image
    {
        get { return _Image; }
        set
        {
            if (value == null)
                _ImageSize = Size.Empty;
            else
                _ImageSize = value.Size;

            _Image = value;
            Invalidate();
        }
    }

    public Bloom[] Colors
    {
        get
        {
            var T = new List<Bloom>();
            Dictionary<string, Color>.Enumerator E = Items.GetEnumerator();

            while (E.MoveNext())
            {
                T.Add(new Bloom(E.Current.Key, E.Current.Value));
            }

            return T.ToArray();
        }
        set
        {
            foreach (Bloom B in value)
            {
                if (Items.ContainsKey(B.Name))
                    Items[B.Name] = B.Value;
            }

            InvalidateCustimization();
            ColorHook();
            Invalidate();
        }
    }

    public string Customization
    {
        get { return _Customization; }
        set
        {
            if (value == _Customization)
                return;

            byte[] Data = null;
            Bloom[] Items = Colors;

            try
            {
                Data = Convert.FromBase64String(value);
                for (int I = 0; I <= Items.Length - 1; I++)
                {
                    Items[I].Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4));
                }
            }
            catch
            {
                return;
            }

            _Customization = value;

            Colors = Items;
            ColorHook();
            Invalidate();
        }
    }

    public bool Transparent
    {
        get { return _Transparent; }
        set
        {
            _Transparent = value;
            if (!(IsHandleCreated || _ControlMode))
                return;

            if (!value && !(BackColor.A == 255))
            {
                throw new Exception("Unable to change value to false while a transparent BackColor is in use.");
            }

            SetStyle(ControlStyles.Opaque, !value);
            SetStyle(ControlStyles.SupportsTransparentBackColor, value);

            InvalidateBitmap();
            Invalidate();
        }
    }

    #endregion

    #region " Private Properties "

    private bool _ControlMode;
    private int _Header = 24;
    private Size _ImageSize;
    private bool _IsAnimated;

    private bool _IsParentForm;
    private int _LockHeight;
    private int _LockWidth;

    protected Size ImageSize
    {
        get { return _ImageSize; }
    }

    protected bool IsParentForm
    {
        get { return _IsParentForm; }
    }

    protected bool IsParentMdi
    {
        get
        {
            if (Parent == null)
                return false;
            return Parent.Parent != null;
        }
    }

    protected int LockWidth
    {
        get { return _LockWidth; }
        set
        {
            _LockWidth = value;
            if (!(LockWidth == 0) && IsHandleCreated)
                Width = LockWidth;
        }
    }

    protected int LockHeight
    {
        get { return _LockHeight; }
        set
        {
            _LockHeight = value;
            if (!(LockHeight == 0) && IsHandleCreated)
                Height = LockHeight;
        }
    }

    protected int Header
    {
        get { return _Header; }
        set
        {
            _Header = value;

            if (!_ControlMode)
            {
                Frame = new Rectangle(7, 7, Width - 14, value - 7);
                Invalidate();
            }
        }
    }

    protected bool ControlMode
    {
        get { return _ControlMode; }
        set
        {
            _ControlMode = value;

            Transparent = _Transparent;
            if (_Transparent && _BackColor)
                BackColor = Color.Transparent;

            InvalidateBitmap();
            Invalidate();
        }
    }

    protected bool IsAnimated
    {
        get { return _IsAnimated; }
        set
        {
            _IsAnimated = value;
            InvalidateTimer();
        }
    }

    #endregion

    #region " Property Helpers "

    protected Pen GetPen(string name)
    {
        return new Pen(Items[name]);
    }

    protected Pen GetPen(string name, float width)
    {
        return new Pen(Items[name], width);
    }

    protected SolidBrush GetBrush(string name)
    {
        return new SolidBrush(Items[name]);
    }

    protected Color GetColor(string name)
    {
        return Items[name];
    }

    protected void SetColor(string name, Color value)
    {
        if (Items.ContainsKey(name))
            Items[name] = value;
        else
            Items.Add(name, value);
    }

    protected void SetColor(string name, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(r, g, b));
    }

    protected void SetColor(string name, byte a, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(a, r, g, b));
    }

    protected void SetColor(string name, byte a, Color value)
    {
        SetColor(name, Color.FromArgb(a, value));
    }

    private void InvalidateBitmap()
    {
        if (_Transparent && _ControlMode)
        {
            if (Width == 0 || Height == 0)
                return;
            B = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
            G = Graphics.FromImage(B);
        }
        else
        {
            G = null;
            B = null;
        }
    }

    private void InvalidateCustimization()
    {
        var M = new MemoryStream(Items.Count * 4);

        foreach (Bloom B in Colors)
        {
            M.Write(BitConverter.GetBytes(B.Value.ToArgb()), 0, 4);
        }

        M.Close();
        _Customization = Convert.ToBase64String(M.ToArray());
    }

    private void InvalidateTimer()
    {
        if (DesignMode || !DoneCreation)
            return;

        if (_IsAnimated)
        {
            ThemeShare.AddAnimationCallback(DoAnimation);
        }
        else
        {
            ThemeShare.RemoveAnimationCallback(DoAnimation);
        }
    }

    #endregion

    #region " User Hooks "

    protected abstract void ColorHook();
    protected abstract void PaintHook();

    protected virtual void OnCreation()
    {
    }

    protected virtual void OnAnimation()
    {
    }

    #endregion

    #region " Offset "

    private Point OffsetReturnPoint;
    private Rectangle OffsetReturnRectangle;

    private Size OffsetReturnSize;

    protected Rectangle Offset(Rectangle r, int amount)
    {
        OffsetReturnRectangle = new Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2));
        return OffsetReturnRectangle;
    }

    protected Size Offset(Size s, int amount)
    {
        OffsetReturnSize = new Size(s.Width + amount, s.Height + amount);
        return OffsetReturnSize;
    }

    protected Point Offset(Point p, int amount)
    {
        OffsetReturnPoint = new Point(p.X + amount, p.Y + amount);
        return OffsetReturnPoint;
    }

    #endregion

    #region " Center "

    private Point CenterReturn;

    protected Point Center(Rectangle p, Rectangle c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X + c.X, (p.Height / 2 - c.Height / 2) + p.Y + c.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle p, Size c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X, (p.Height / 2 - c.Height / 2) + p.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }

    protected Point Center(Size child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }

    protected Point Center(int childWidth, int childHeight)
    {
        return Center(Width, Height, childWidth, childHeight);
    }

    protected Point Center(Size p, Size c)
    {
        return Center(p.Width, p.Height, c.Width, c.Height);
    }

    protected Point Center(int pWidth, int pHeight, int cWidth, int cHeight)
    {
        CenterReturn = new Point(pWidth / 2 - cWidth / 2, pHeight / 2 - cHeight / 2);
        return CenterReturn;
    }

    #endregion

    #region " Measure "

    private readonly Graphics MeasureGraphics;
    private Bitmap MeasureBitmap;

    protected Size Measure()
    {
        lock (MeasureGraphics)
        {
            return MeasureGraphics.MeasureString(Text, Font, Width).ToSize();
        }
    }

    protected Size Measure(string text)
    {
        lock (MeasureGraphics)
        {
            return MeasureGraphics.MeasureString(text, Font, Width).ToSize();
        }
    }

    #endregion

    #region " DrawPixel "

    private SolidBrush DrawPixelBrush;

    protected void DrawPixel(Color c1, int x, int y)
    {
        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
        }
        else
        {
            DrawPixelBrush = new SolidBrush(c1);
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1);
        }
    }

    #endregion

    #region " DrawCorners "

    private SolidBrush DrawCornersBrush;

    protected void DrawCorners(Color c1, int offset)
    {
        DrawCorners(c1, 0, 0, Width, Height, offset);
    }

    protected void DrawCorners(Color c1, Rectangle r1, int offset)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset);
    }

    protected void DrawCorners(Color c1, int x, int y, int width, int height, int offset)
    {
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawCorners(Color c1)
    {
        DrawCorners(c1, 0, 0, Width, Height);
    }

    protected void DrawCorners(Color c1, Rectangle r1)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height);
    }

    protected void DrawCorners(Color c1, int x, int y, int width, int height)
    {
        if (_NoRounding)
            return;

        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
            B.SetPixel(x + (width - 1), y, c1);
            B.SetPixel(x, y + (height - 1), c1);
            B.SetPixel(x + (width - 1), y + (height - 1), c1);
        }
        else
        {
            DrawCornersBrush = new SolidBrush(c1);
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1);
        }
    }

    #endregion

    #region " DrawBorders "

    protected void DrawBorders(Pen p1, int offset)
    {
        DrawBorders(p1, 0, 0, Width, Height, offset);
    }

    protected void DrawBorders(Pen p1, Rectangle r, int offset)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset);
    }

    protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset)
    {
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawBorders(Pen p1)
    {
        DrawBorders(p1, 0, 0, Width, Height);
    }

    protected void DrawBorders(Pen p1, Rectangle r)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height);
    }

    protected void DrawBorders(Pen p1, int x, int y, int width, int height)
    {
        G.DrawRectangle(p1, x, y, width - 1, height - 1);
    }

    #endregion

    #region " DrawText "

    private Point DrawTextPoint;

    private Size DrawTextSize;

    protected void DrawText(Brush b1, HorizontalAlignment a, int x, int y)
    {
        DrawText(b1, Text, a, x, y);
    }

    protected void DrawText(Brush b1, string text, HorizontalAlignment a, int x, int y)
    {
        if (text.Length == 0)
            return;

        DrawTextSize = Measure(text);
        DrawTextPoint = new Point(Width / 2 - DrawTextSize.Width / 2, Header / 2 - DrawTextSize.Height / 2);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y);
                break;
        }
    }

    protected void DrawText(Brush b1, Point p1)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, p1);
    }

    protected void DrawText(Brush b1, int x, int y)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, x, y);
    }

    #endregion

    #region " DrawImage "

    private Point DrawImagePoint;

    protected void DrawImage(HorizontalAlignment a, int x, int y)
    {
        DrawImage(_Image, a, x, y);
    }

    protected void DrawImage(Image image, HorizontalAlignment a, int x, int y)
    {
        if (image == null)
            return;
        DrawImagePoint = new Point(Width / 2 - image.Width / 2, Header / 2 - image.Height / 2);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
        }
    }

    protected void DrawImage(Point p1)
    {
        DrawImage(_Image, p1.X, p1.Y);
    }

    protected void DrawImage(int x, int y)
    {
        DrawImage(_Image, x, y);
    }

    protected void DrawImage(Image image, Point p1)
    {
        DrawImage(image, p1.X, p1.Y);
    }

    protected void DrawImage(Image image, int x, int y)
    {
        if (image == null)
            return;
        G.DrawImage(image, x, y, image.Width, image.Height);
    }

    #endregion

    #region " DrawGradient "

    private LinearGradientBrush DrawGradientBrush;

    private Rectangle DrawGradientRectangle;

    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle);
    }

    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, 90f);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, angle);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }


    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle);
    }

    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, angle);
        G.FillRectangle(DrawGradientBrush, r);
    }

    #endregion

    #region " DrawRadial "

    private readonly GraphicsPath DrawRadialPath;
    private PathGradientBrush DrawRadialBrush1;
    private LinearGradientBrush DrawRadialBrush2;

    private Rectangle DrawRadialRectangle;

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, width / 2, height / 2);
    }

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, Point center)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y);
    }

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, int cx, int cy)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, cx, cy);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r)
    {
        DrawRadial(blend, r, r.Width / 2, r.Height / 2);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r, Point center)
    {
        DrawRadial(blend, r, center.X, center.Y);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r, int cx, int cy)
    {
        DrawRadialPath.Reset();
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1);

        DrawRadialBrush1 = new PathGradientBrush(DrawRadialPath);
        DrawRadialBrush1.CenterPoint = new Point(r.X + cx, r.Y + cy);
        DrawRadialBrush1.InterpolationColors = blend;

        if (G.SmoothingMode == SmoothingMode.AntiAlias)
        {
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3);
        }
        else
        {
            G.FillEllipse(DrawRadialBrush1, r);
        }
    }


    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawGradientRectangle);
    }

    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, angle);
        G.FillEllipse(DrawGradientBrush, r);
    }

    #endregion

    #region " CreateRound "

    private GraphicsPath CreateRoundPath;

    private Rectangle CreateRoundRectangle;

    public GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
    {
        CreateRoundRectangle = new Rectangle(x, y, width, height);
        return CreateRound(CreateRoundRectangle, slope);
    }

    public GraphicsPath CreateRound(Rectangle r, int slope)
    {
        CreateRoundPath = new GraphicsPath(FillMode.Winding);
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
        CreateRoundPath.CloseFigure();
        return CreateRoundPath;
    }

    #endregion
}

internal abstract class ThemeControl154 : Control
{
    #region " Initialization "

    protected Bitmap B;
    private bool DoneCreation;
    protected Graphics G;

    public ThemeControl154()
    {
        SetStyle((ControlStyles)139270, true);

        _ImageSize = Size.Empty;
        Font = new Font("Verdana", 8);

        MeasureBitmap = new Bitmap(1, 1);
        MeasureGraphics = Graphics.FromImage(MeasureBitmap);

        DrawRadialPath = new GraphicsPath();

        InvalidateCustimization();
        //Remove?
    }

    protected override sealed void OnHandleCreated(EventArgs e)
    {
        InvalidateCustimization();
        ColorHook();

        if (!(_LockWidth == 0))
            Width = _LockWidth;
        if (!(_LockHeight == 0))
            Height = _LockHeight;

        Transparent = _Transparent;
        if (_Transparent && _BackColor)
            BackColor = Color.Transparent;

        base.OnHandleCreated(e);
    }

    protected override sealed void OnParentChanged(EventArgs e)
    {
        if (Parent != null)
        {
            OnCreation();
            DoneCreation = true;
            InvalidateTimer();
        }

        base.OnParentChanged(e);
    }

    #endregion

    private void DoAnimation(bool i)
    {
        OnAnimation();
        if (i)
            Invalidate();
    }

    protected override sealed void OnPaint(PaintEventArgs e)
    {
        if (Width == 0 || Height == 0)
            return;

        if (_Transparent)
        {
            PaintHook();
            e.Graphics.DrawImage(B, 0, 0);
        }
        else
        {
            G = e.Graphics;
            PaintHook();
        }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        ThemeShare.RemoveAnimationCallback(DoAnimation);
        base.OnHandleDestroyed(e);
    }

    #region " Size Handling "

    protected override sealed void OnSizeChanged(EventArgs e)
    {
        if (_Transparent)
        {
            InvalidateBitmap();
        }

        Invalidate();
        base.OnSizeChanged(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        if (!(_LockWidth == 0))
            width = _LockWidth;
        if (!(_LockHeight == 0))
            height = _LockHeight;
        base.SetBoundsCore(x, y, width, height, specified);
    }

    #endregion

    #region " State Handling "

    private bool InPosition;
    protected MouseState State;

    protected override void OnMouseEnter(EventArgs e)
    {
        InPosition = true;
        SetState(MouseState.Over);
        base.OnMouseEnter(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (InPosition)
            SetState(MouseState.Over);
        base.OnMouseUp(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            SetState(MouseState.Down);
        base.OnMouseDown(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        InPosition = false;
        SetState(MouseState.None);
        base.OnMouseLeave(e);
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        if (Enabled)
            SetState(MouseState.None);
        else
            SetState(MouseState.Block);
        base.OnEnabledChanged(e);
    }

    private void SetState(MouseState current)
    {
        State = current;
        Invalidate();
    }

    #endregion

    #region " Base Properties "

    private bool _BackColor;

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Color ForeColor
    {
        get { return Color.Empty; }
        set { }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override Image BackgroundImage
    {
        get { return null; }
        set { }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ImageLayout BackgroundImageLayout
    {
        get { return ImageLayout.None; }
        set { }
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            Invalidate();
        }
    }

    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            Invalidate();
        }
    }

    [Category("Misc")]
    public override Color BackColor
    {
        get { return base.BackColor; }
        set
        {
            if (!IsHandleCreated && value == Color.Transparent)
            {
                _BackColor = true;
                return;
            }

            base.BackColor = value;
            if (Parent != null)
                ColorHook();
        }
    }

    #endregion

    #region " Public Properties "

    private readonly Dictionary<string, Color> Items = new Dictionary<string, Color>();
    private string _Customization;
    private Image _Image;
    private bool _NoRounding;
    private bool _Transparent;

    public bool NoRounding
    {
        get { return _NoRounding; }
        set
        {
            _NoRounding = value;
            Invalidate();
        }
    }

    public Image Image
    {
        get { return _Image; }
        set
        {
            if (value == null)
            {
                _ImageSize = Size.Empty;
            }
            else
            {
                _ImageSize = value.Size;
            }

            _Image = value;
            Invalidate();
        }
    }

    public bool Transparent
    {
        get { return _Transparent; }
        set
        {
            _Transparent = value;
            if (!IsHandleCreated)
                return;

            if (!value && !(BackColor.A == 255))
            {
                throw new Exception("Unable to change value to false while a transparent BackColor is in use.");
            }

            SetStyle(ControlStyles.Opaque, !value);
            SetStyle(ControlStyles.SupportsTransparentBackColor, value);

            if (value)
                InvalidateBitmap();
            else
                B = null;
            Invalidate();
        }
    }

    public Bloom[] Colors
    {
        get
        {
            var T = new List<Bloom>();
            Dictionary<string, Color>.Enumerator E = Items.GetEnumerator();

            while (E.MoveNext())
            {
                T.Add(new Bloom(E.Current.Key, E.Current.Value));
            }

            return T.ToArray();
        }
        set
        {
            foreach (Bloom B in value)
            {
                if (Items.ContainsKey(B.Name))
                    Items[B.Name] = B.Value;
            }

            InvalidateCustimization();
            ColorHook();
            Invalidate();
        }
    }

    public string Customization
    {
        get { return _Customization; }
        set
        {
            if (value == _Customization)
                return;

            byte[] Data = null;
            Bloom[] Items = Colors;

            try
            {
                Data = Convert.FromBase64String(value);
                for (int I = 0; I <= Items.Length - 1; I++)
                {
                    Items[I].Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4));
                }
            }
            catch
            {
                return;
            }

            _Customization = value;

            Colors = Items;
            ColorHook();
            Invalidate();
        }
    }

    #endregion

    #region " Private Properties "

    private Size _ImageSize;
    private bool _IsAnimated;
    private int _LockHeight;

    private int _LockWidth;

    protected Size ImageSize
    {
        get { return _ImageSize; }
    }

    protected int LockWidth
    {
        get { return _LockWidth; }
        set
        {
            _LockWidth = value;
            if (!(LockWidth == 0) && IsHandleCreated)
                Width = LockWidth;
        }
    }

    protected int LockHeight
    {
        get { return _LockHeight; }
        set
        {
            _LockHeight = value;
            if (!(LockHeight == 0) && IsHandleCreated)
                Height = LockHeight;
        }
    }

    protected bool IsAnimated
    {
        get { return _IsAnimated; }
        set
        {
            _IsAnimated = value;
            InvalidateTimer();
        }
    }

    #endregion

    #region " Property Helpers "

    protected Pen GetPen(string name)
    {
        return new Pen(Items[name]);
    }

    protected Pen GetPen(string name, float width)
    {
        return new Pen(Items[name], width);
    }

    protected SolidBrush GetBrush(string name)
    {
        return new SolidBrush(Items[name]);
    }

    protected Color GetColor(string name)
    {
        return Items[name];
    }

    protected void SetColor(string name, Color value)
    {
        if (Items.ContainsKey(name))
            Items[name] = value;
        else
            Items.Add(name, value);
    }

    protected void SetColor(string name, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(r, g, b));
    }

    protected void SetColor(string name, byte a, byte r, byte g, byte b)
    {
        SetColor(name, Color.FromArgb(a, r, g, b));
    }

    protected void SetColor(string name, byte a, Color value)
    {
        SetColor(name, Color.FromArgb(a, value));
    }

    private void InvalidateBitmap()
    {
        if (Width == 0 || Height == 0)
            return;
        B = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
        G = Graphics.FromImage(B);
    }

    private void InvalidateCustimization()
    {
        var M = new MemoryStream(Items.Count * 4);

        foreach (Bloom B in Colors)
        {
            M.Write(BitConverter.GetBytes(B.Value.ToArgb()), 0, 4);
        }

        M.Close();
        _Customization = Convert.ToBase64String(M.ToArray());
    }

    private void InvalidateTimer()
    {
        if (DesignMode || !DoneCreation)
            return;

        if (_IsAnimated)
        {
            ThemeShare.AddAnimationCallback(DoAnimation);
        }
        else
        {
            ThemeShare.RemoveAnimationCallback(DoAnimation);
        }
    }

    #endregion

    #region " User Hooks "

    protected abstract void ColorHook();
    protected abstract void PaintHook();

    protected virtual void OnCreation()
    {
    }

    protected virtual void OnAnimation()
    {
    }

    #endregion

    #region " Offset "

    private Point OffsetReturnPoint;
    private Rectangle OffsetReturnRectangle;

    private Size OffsetReturnSize;

    protected Rectangle Offset(Rectangle r, int amount)
    {
        OffsetReturnRectangle = new Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2));
        return OffsetReturnRectangle;
    }

    protected Size Offset(Size s, int amount)
    {
        OffsetReturnSize = new Size(s.Width + amount, s.Height + amount);
        return OffsetReturnSize;
    }

    protected Point Offset(Point p, int amount)
    {
        OffsetReturnPoint = new Point(p.X + amount, p.Y + amount);
        return OffsetReturnPoint;
    }

    #endregion

    #region " Center "

    private Point CenterReturn;

    protected Point Center(Rectangle p, Rectangle c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X + c.X, (p.Height / 2 - c.Height / 2) + p.Y + c.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle p, Size c)
    {
        CenterReturn = new Point((p.Width / 2 - c.Width / 2) + p.X, (p.Height / 2 - c.Height / 2) + p.Y);
        return CenterReturn;
    }

    protected Point Center(Rectangle child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }

    protected Point Center(Size child)
    {
        return Center(Width, Height, child.Width, child.Height);
    }

    protected Point Center(int childWidth, int childHeight)
    {
        return Center(Width, Height, childWidth, childHeight);
    }

    protected Point Center(Size p, Size c)
    {
        return Center(p.Width, p.Height, c.Width, c.Height);
    }

    protected Point Center(int pWidth, int pHeight, int cWidth, int cHeight)
    {
        CenterReturn = new Point(pWidth / 2 - cWidth / 2, pHeight / 2 - cHeight / 2);
        return CenterReturn;
    }

    #endregion

    #region " Measure "

    private readonly Graphics MeasureGraphics;
    private Bitmap MeasureBitmap;

    protected Size Measure()
    {
        return MeasureGraphics.MeasureString(Text, Font, Width).ToSize();
    }

    protected Size Measure(string text)
    {
        return MeasureGraphics.MeasureString(text, Font, Width).ToSize();
    }

    #endregion

    #region " DrawPixel "

    private SolidBrush DrawPixelBrush;

    protected void DrawPixel(Color c1, int x, int y)
    {
        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
        }
        else
        {
            DrawPixelBrush = new SolidBrush(c1);
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1);
        }
    }

    #endregion

    #region " DrawCorners "

    private SolidBrush DrawCornersBrush;

    protected void DrawCorners(Color c1, int offset)
    {
        DrawCorners(c1, 0, 0, Width, Height, offset);
    }

    protected void DrawCorners(Color c1, Rectangle r1, int offset)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset);
    }

    protected void DrawCorners(Color c1, int x, int y, int width, int height, int offset)
    {
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawCorners(Color c1)
    {
        DrawCorners(c1, 0, 0, Width, Height);
    }

    protected void DrawCorners(Color c1, Rectangle r1)
    {
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height);
    }

    protected void DrawCorners(Color c1, int x, int y, int width, int height)
    {
        if (_NoRounding)
            return;

        if (_Transparent)
        {
            B.SetPixel(x, y, c1);
            B.SetPixel(x + (width - 1), y, c1);
            B.SetPixel(x, y + (height - 1), c1);
            B.SetPixel(x + (width - 1), y + (height - 1), c1);
        }
        else
        {
            DrawCornersBrush = new SolidBrush(c1);
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1);
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1);
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1);
        }
    }

    #endregion

    #region " DrawBorders "

    protected void DrawBorders(Pen p1, int offset)
    {
        DrawBorders(p1, 0, 0, Width, Height, offset);
    }

    protected void DrawBorders(Pen p1, Rectangle r, int offset)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset);
    }

    protected void DrawBorders(Pen p1, int x, int y, int width, int height, int offset)
    {
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2));
    }

    protected void DrawBorders(Pen p1)
    {
        DrawBorders(p1, 0, 0, Width, Height);
    }

    protected void DrawBorders(Pen p1, Rectangle r)
    {
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height);
    }

    protected void DrawBorders(Pen p1, int x, int y, int width, int height)
    {
        G.DrawRectangle(p1, x, y, width - 1, height - 1);
    }

    #endregion

    #region " DrawText "

    private Point DrawTextPoint;

    private Size DrawTextSize;

    protected void DrawText(Brush b1, HorizontalAlignment a, int x, int y)
    {
        DrawText(b1, Text, a, x, y);
    }

    protected void DrawText(Brush b1, string text, HorizontalAlignment a, int x, int y)
    {
        if (text.Length == 0)
            return;

        DrawTextSize = Measure(text);
        DrawTextPoint = Center(DrawTextSize);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Center:
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y);
                break;
            case HorizontalAlignment.Right:
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y);
                break;
        }
    }

    protected void DrawText(Brush b1, Point p1)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, p1);
    }

    protected void DrawText(Brush b1, int x, int y)
    {
        if (Text.Length == 0)
            return;
        G.DrawString(Text, Font, b1, x, y);
    }

    #endregion

    #region " DrawImage "

    private Point DrawImagePoint;

    protected void DrawImage(HorizontalAlignment a, int x, int y)
    {
        DrawImage(_Image, a, x, y);
    }

    protected void DrawImage(Image image, HorizontalAlignment a, int x, int y)
    {
        if (image == null)
            return;
        DrawImagePoint = Center(image.Size);

        switch (a)
        {
            case HorizontalAlignment.Left:
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Center:
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
            case HorizontalAlignment.Right:
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height);
                break;
        }
    }

    protected void DrawImage(Point p1)
    {
        DrawImage(_Image, p1.X, p1.Y);
    }

    protected void DrawImage(int x, int y)
    {
        DrawImage(_Image, x, y);
    }

    protected void DrawImage(Image image, Point p1)
    {
        DrawImage(image, p1.X, p1.Y);
    }

    protected void DrawImage(Image image, int x, int y)
    {
        if (image == null)
            return;
        G.DrawImage(image, x, y, image.Width, image.Height);
    }

    #endregion

    #region " DrawGradient "

    private LinearGradientBrush DrawGradientBrush;

    private Rectangle DrawGradientRectangle;

    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle);
    }

    protected void DrawGradient(ColorBlend blend, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(blend, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, 90f);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }

    protected void DrawGradient(ColorBlend blend, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, Color.Empty, Color.Empty, angle);
        DrawGradientBrush.InterpolationColors = blend;
        G.FillRectangle(DrawGradientBrush, r);
    }


    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle);
    }

    protected void DrawGradient(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawGradientRectangle = new Rectangle(x, y, width, height);
        DrawGradient(c1, c2, DrawGradientRectangle, angle);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillRectangle(DrawGradientBrush, r);
    }

    protected void DrawGradient(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawGradientBrush = new LinearGradientBrush(r, c1, c2, angle);
        G.FillRectangle(DrawGradientBrush, r);
    }

    #endregion

    #region " DrawRadial "

    private readonly GraphicsPath DrawRadialPath;
    private PathGradientBrush DrawRadialBrush1;
    private LinearGradientBrush DrawRadialBrush2;

    private Rectangle DrawRadialRectangle;

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, width / 2, height / 2);
    }

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, Point center)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y);
    }

    public void DrawRadial(ColorBlend blend, int x, int y, int width, int height, int cx, int cy)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(blend, DrawRadialRectangle, cx, cy);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r)
    {
        DrawRadial(blend, r, r.Width / 2, r.Height / 2);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r, Point center)
    {
        DrawRadial(blend, r, center.X, center.Y);
    }

    public void DrawRadial(ColorBlend blend, Rectangle r, int cx, int cy)
    {
        DrawRadialPath.Reset();
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1);

        DrawRadialBrush1 = new PathGradientBrush(DrawRadialPath);
        DrawRadialBrush1.CenterPoint = new Point(r.X + cx, r.Y + cy);
        DrawRadialBrush1.InterpolationColors = blend;

        if (G.SmoothingMode == SmoothingMode.AntiAlias)
        {
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3);
        }
        else
        {
            G.FillEllipse(DrawRadialBrush1, r);
        }
    }


    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawRadialRectangle);
    }

    protected void DrawRadial(Color c1, Color c2, int x, int y, int width, int height, float angle)
    {
        DrawRadialRectangle = new Rectangle(x, y, width, height);
        DrawRadial(c1, c2, DrawRadialRectangle, angle);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, 90f);
        G.FillEllipse(DrawRadialBrush2, r);
    }

    protected void DrawRadial(Color c1, Color c2, Rectangle r, float angle)
    {
        DrawRadialBrush2 = new LinearGradientBrush(r, c1, c2, angle);
        G.FillEllipse(DrawRadialBrush2, r);
    }

    #endregion

    #region " CreateRound "

    private GraphicsPath CreateRoundPath;

    private Rectangle CreateRoundRectangle;

    public GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
    {
        CreateRoundRectangle = new Rectangle(x, y, width, height);
        return CreateRound(CreateRoundRectangle, slope);
    }

    public GraphicsPath CreateRound(Rectangle r, int slope)
    {
        CreateRoundPath = new GraphicsPath(FillMode.Winding);
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
        CreateRoundPath.CloseFigure();
        return CreateRoundPath;
    }

    #endregion
}

internal static class ThemeShare
{
    #region " Animation "

    public delegate void AnimationDelegate(bool invalidate);

    private const int FPS = 50;

    private const int Rate = 10;

    private static int Frames;
    private static bool Invalidate;

    public static PrecisionTimer ThemeTimer = new PrecisionTimer();
    //1000 / 50 = 20 FPS


    private static readonly List<AnimationDelegate> Callbacks = new List<AnimationDelegate>();

    private static void HandleCallbacks(IntPtr state, bool reserve)
    {
        Invalidate = (Frames >= FPS);
        if (Invalidate)
            Frames = 0;

        lock (Callbacks)
        {
            for (int I = 0; I <= Callbacks.Count - 1; I++)
            {
                Callbacks[I].Invoke(Invalidate);
            }
        }

        Frames += Rate;
    }

    private static void InvalidateThemeTimer()
    {
        if (Callbacks.Count == 0)
        {
            ThemeTimer.Delete();
        }
        else
        {
            ThemeTimer.Create(0, Rate, HandleCallbacks);
        }
    }

    public static void AddAnimationCallback(AnimationDelegate callback)
    {
        lock (Callbacks)
        {
            if (Callbacks.Contains(callback))
                return;

            Callbacks.Add(callback);
            InvalidateThemeTimer();
        }
    }

    public static void RemoveAnimationCallback(AnimationDelegate callback)
    {
        lock (Callbacks)
        {
            if (!Callbacks.Contains(callback))
                return;

            Callbacks.Remove(callback);
            InvalidateThemeTimer();
        }
    }

    #endregion
}

internal enum MouseState : byte
{
    None = 0,
    Over = 1,
    Down = 2,
    Block = 3
}

internal struct Bloom
{
    public string _Name;

    private Color _Value;

    public Bloom(string name, Color value)
    {
        _Name = name;
        _Value = value;
    }

    public string Name
    {
        get { return _Name; }
    }

    public Color Value
    {
        get { return _Value; }
        set { _Value = value; }
    }

    public string ValueHex
    {
        get
        {
            return string.Concat("#", _Value.R.ToString("X2", null), _Value.G.ToString("X2", null),
                _Value.B.ToString("X2", null));
        }
        set
        {
            try
            {
                _Value = ColorTranslator.FromHtml(value);
            }
            catch
            {
            }
        }
    }
}

//------------------
//Creator: aeonhack
//Site: elitevs.net
//Created: 11/30/2011
//Changed: 11/30/2011
//Version: 1.0.0
//------------------
internal class PrecisionTimer : IDisposable
{
    public delegate void TimerDelegate(IntPtr r1, bool r2);

    private IntPtr Handle;

    private TimerDelegate TimerCallback;
    private bool _Enabled;

    public bool Enabled
    {
        get { return _Enabled; }
    }

    public void Dispose()
    {
        Delete();
    }

    [DllImport("kernel32.dll", EntryPoint = "CreateTimerQueueTimer")]
    private static extern bool CreateTimerQueueTimer(ref IntPtr handle, IntPtr queue, TimerDelegate callback,
        IntPtr state, uint dueTime, uint period, uint flags);

    [DllImport("kernel32.dll", EntryPoint = "DeleteTimerQueueTimer")]
    private static extern bool DeleteTimerQueueTimer(IntPtr queue, IntPtr handle, IntPtr callback);

    public void Create(uint dueTime, uint period, TimerDelegate callback)
    {
        if (_Enabled)
            return;

        TimerCallback = callback;
        bool Success = CreateTimerQueueTimer(ref Handle, IntPtr.Zero, TimerCallback, IntPtr.Zero, dueTime, period, 0);

        if (!Success)
            ThrowNewException("CreateTimerQueueTimer");
        _Enabled = Success;
    }

    public void Delete()
    {
        if (!_Enabled)
            return;
        bool Success = DeleteTimerQueueTimer(IntPtr.Zero, Handle, IntPtr.Zero);

        if (!Success && !(Marshal.GetLastWin32Error() == 997))
        {
            ThrowNewException("DeleteTimerQueueTimer");
        }

        _Enabled = !Success;
    }

    private void ThrowNewException(string name)
    {
        throw new Exception(string.Format("{0} failed. Win32Error: {1}", name, Marshal.GetLastWin32Error()));
    }
}

internal class PatcherTheme : ThemeContainer154
{
    private Color BGGradient1;
    private Color BGGradient2;

    private Pen ControlBarBorder;
    private Color ControlBarBorderLight;

    public PatcherTheme()
    {
        TransparencyKey = Color.Fuchsia;
        MinimumSize = new Size(150, 150);
        Font = new Font("Segoe UI", 9);
        BackColor = Color.FromArgb(246, 246, 246);
        SetColor("BGGradient1", Color.FromArgb(46, 46, 46));
        SetColor("BGGradient2", Color.FromArgb(33, 33, 33));

        SetColor("ControlBarBorderLight", Color.FromArgb(85, 85, 85));
    }

    protected override void ColorHook()
    {
        BGGradient1 = GetColor("BGGradient1");
        BGGradient2 = GetColor("BGGradient2");

        ControlBarBorderLight = GetColor("ControlBarBorderLight");

        var aGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(Width, 0), ControlBarBorderLight,
            Color.FromArgb(0, 0, 0, 0));
        var cb = new ColorBlend();
        cb.Positions = new[] { 0, 0.5f, 1f };
        cb.Colors = new[] { Color.FromArgb(0, 0, 0, 0), ControlBarBorderLight, Color.FromArgb(0, 0, 0, 0) };
        aGradientBrush.InterpolationColors = cb;
        ControlBarBorder = new Pen(aGradientBrush);
    }

    protected override void PaintHook()
    {
        G.Clear(BackColor);
        // Dark BG
        G.SmoothingMode = SmoothingMode.AntiAlias;
        LinearGradientBrush BGGradientDark;
        BGGradientDark = new LinearGradientBrush(new Rectangle(-1, -1, Width, 28), BGGradient1, BGGradient2, 90, false);
        BGGradientDark.SetSigmaBellShape(1f, 0.80f);
        G.FillRectangle(BGGradientDark, new Rectangle(-1, -1, Width + 1, 28));
        G.DrawLine(new Pen(Color.FromArgb(27, 27, 27)), 0, 0, Width + 1, 0);
        G.DrawLine(ControlBarBorder, 0, 1, Width, 1);

        // Borders
        G.DrawLine(Pens.DarkGray, 0, 27, 0, Height);
        G.DrawLine(Pens.DarkGray, 0, Height - 1, Width - 1, Height - 1);
        G.DrawLine(Pens.DarkGray, Width - 1, 27, Width - 1, Height - 1);

        // Top Details
        G.DrawIcon(FindForm().Icon, new Rectangle(5, 5, 20, 20));
        var format = new StringFormat();
        format.LineAlignment = StringAlignment.Center;
        format.Alignment = StringAlignment.Center;
        G.DrawString(FindForm().Text, new Font("Segoe UI", 10, FontStyle.Bold), Brushes.White,
            new RectangleF(0, 3, Width - 1, 25), format);
    }
}

internal class PatcherButton : ThemeControl154
{
    private Color MiddleLine;
    private Color OutsideLine;
    private Color Stop1;
    private Color Stop2;

    public PatcherButton()
    {
        SetColor("Stop1", Color.FromArgb(255, 185, 0));
        SetColor("Stop2", Color.FromArgb(254, 112, 0));

        SetColor("MiddleLine", Color.FromArgb(255, 255, 0));
        SetColor("OutsideLine", Color.FromArgb(255, 227, 0));

        Size = new Size(97, 31);
    }

    [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
    private static extern IntPtr CreateRoundRectRgn
        (
        int nLeftRect, // x-coordinate of upper-left corner
        int nTopRect, // y-coordinate of upper-left corner
        int nRightRect, // x-coordinate of lower-right corner
        int nBottomRect, // y-coordinate of lower-right corner
        int nWidthEllipse, // height of ellipse
        int nHeightEllipse // width of ellipse
        );

    protected override void ColorHook()
    {
        Stop1 = GetColor("Stop1");
        Stop2 = GetColor("Stop2");

        MiddleLine = GetColor("MiddleLine");
        OutsideLine = GetColor("OutsideLine");
    }

    protected override void PaintHook()
    {
        SetRegion();
        switch (State)
        {
            case MouseState.None:
                var lgb = new LinearGradientBrush(ClientRectangle, Stop1, Stop2, 90);
                lgb.SetSigmaBellShape(1f, 0.80f);
                G.FillRectangle(lgb, ClientRectangle);
                var eg = new ExtendedGraphics(G);

                var lbg2 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(255, 165, 0),
                    Color.FromArgb(253, 68, 0), 90);
                var Bor = new Pen(lbg2);
                G.DrawPath(Bor, eg.GetRoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), 1.5f));

                var aGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(Width, 0), MiddleLine,
                    OutsideLine);
                var cb = new ColorBlend();
                cb.Positions = new[] { 0, 0.3f, 0.5f, 0.70f, 1f };
                cb.Colors = new[] { Color.Transparent, OutsideLine, MiddleLine, OutsideLine, Color.Transparent };
                aGradientBrush.InterpolationColors = cb;
                var TopLine = new Pen(aGradientBrush);

                G.DrawLine(TopLine, new Point(10, 1), new Point(Width - 11, 1));

                var format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
                G.DrawString(Text, new Font("Segoe UI", 10, FontStyle.Bold), Brushes.White,
                    new RectangleF(0, 1, Width - 1, Height - 1), format);
                break;


            case MouseState.Down:
                var lgb3 = new LinearGradientBrush(ClientRectangle, Stop2, Stop1, 90);
                lgb3.SetSigmaBellShape(1f, 0.80f);
                G.FillRectangle(lgb3, ClientRectangle);
                var eg2 = new ExtendedGraphics(G);

                var lbg4 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(208, 65, 0),
                    Color.FromArgb(234, 128, 0), 90);
                var Bor2 = new Pen(lbg4);
                G.DrawPath(Bor2, eg2.GetRoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), 1.5f));

                G.DrawLine(new Pen(Color.FromArgb(242, 194, 0)), new Point(33, Height - 2),
                    new Point(Width - 34, Height - 2));

                var format2 = new StringFormat();
                format2.LineAlignment = StringAlignment.Center;
                format2.Alignment = StringAlignment.Center;
                G.DrawString(Text, new Font("Segoe UI", 10, FontStyle.Bold), Brushes.White,
                    new RectangleF(0, 1, Width - 1, Height - 1), format2);
                break;

            case MouseState.Over:
                var lgb5 = new LinearGradientBrush(ClientRectangle, Color.FromArgb(255, 213, 0), Stop2, 90);
                lgb5.SetSigmaBellShape(1f, 0.80f);
                G.FillRectangle(lgb5, ClientRectangle);
                var eg3 = new ExtendedGraphics(G);

                var lbg6 = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), Color.FromArgb(255, 165, 0),
                    Color.FromArgb(253, 68, 0), 90);
                var Bor3 = new Pen(lbg6);
                G.DrawPath(Bor3, eg3.GetRoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), 1.5f));

                var aGradientBrush3 = new LinearGradientBrush(new Point(0, 0), new Point(Width, 0), MiddleLine,
                    OutsideLine);
                var cb3 = new ColorBlend();
                cb3.Positions = new[] { 0, 0.3f, 0.5f, 0.70f, 1f };
                cb3.Colors = new[] { Color.Transparent, OutsideLine, MiddleLine, OutsideLine, Color.Transparent };
                aGradientBrush3.InterpolationColors = cb3;
                var TopLine3 = new Pen(aGradientBrush3);

                G.DrawLine(TopLine3, new Point(10, 1), new Point(Width - 11, 1));

                var format3 = new StringFormat();
                format3.LineAlignment = StringAlignment.Center;
                format3.Alignment = StringAlignment.Center;
                G.DrawString(Text, new Font("Segoe UI", 10, FontStyle.Bold), Brushes.White,
                    new RectangleF(0, 1, Width - 1, Height - 1), format3);
                break;
        }
    }

    private void SetRegion()
    {
        Region = GetRoundedRegion(Width, Height);
        G.InterpolationMode = InterpolationMode.HighQualityBilinear;
        G.CompositingQuality = CompositingQuality.HighQuality;
        G.SmoothingMode = SmoothingMode.AntiAlias;
    }

    private Region GetRoundedRegion(int controlWidth, int controlHeight)
    {
        return Region.FromHrgn(CreateRoundRectRgn(0, 0, controlWidth + 1, controlHeight + 1, 3, 3));
    }
}


internal class CloseButton : ThemeControl154
{
    public CloseButton()
    {
        MaximumSize = new Size(18, 18);
        MinimumSize = MaximumSize;
        Anchor = AnchorStyles.Right | AnchorStyles.Top;
    }

    [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
    private static extern IntPtr CreateRoundRectRgn
        (
        int nLeftRect, // x-coordinate of upper-left corner
        int nTopRect, // y-coordinate of upper-left corner
        int nRightRect, // x-coordinate of lower-right corner
        int nBottomRect, // y-coordinate of lower-right corner
        int nWidthEllipse, // height of ellipse
        int nHeightEllipse // width of ellipse
        );

    protected override void OnClick(EventArgs e)
    {
        FindForm().Close();
    }

    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        SetRegion();
        var BigPen = new Pen(Color.FromArgb(161, 161, 161));
        BigPen.Width = 1.8f;
        BigPen.SetLineCap(LineCap.Triangle, LineCap.Triangle, DashCap.Round);
        switch (State)
        {
            case MouseState.None:
                var BGGradientDark = new LinearGradientBrush(new Rectangle(-1, -1, Width, 28),
                    Color.FromArgb(46, 46, 46), Color.FromArgb(33, 33, 33), 90, false);
                BGGradientDark.SetSigmaBellShape(1f, 0.80f);
                G.FillRectangle(BGGradientDark, new Rectangle(-1, -1, Width + 1, 28));
                G.DrawLine(BigPen, 5.5f, 5.5f, 11.5f, 11.5f);
                G.DrawLine(BigPen, 11.5f, 5.5f, 5.5f, 11.5f);
                break;
            case MouseState.Over:
                G.Clear(Color.FromArgb(75, 75, 75));
                G.DrawLine(BigPen, 5.5f, 5.5f, 11.5f, 11.5f);
                G.DrawLine(BigPen, 11.5f, 5.5f, 5.5f, 11.5f);
                break;
        }
    }

    private void SetRegion()
    {
        Region = GetRoundedRegion(Width, Height);
        G.InterpolationMode = InterpolationMode.HighQualityBilinear;
        G.CompositingQuality = CompositingQuality.HighQuality;
        G.SmoothingMode = SmoothingMode.AntiAlias;
    }

    private Region GetRoundedRegion(int controlWidth, int controlHeight)
    {
        return Region.FromHrgn(CreateRoundRectRgn(0, 0, controlWidth + 1, controlHeight + 1, 3, 3));
    }
}


internal class MinimizeButton : ThemeControl154
{
    public MinimizeButton()
    {
        MaximumSize = new Size(18, 18);
        MinimumSize = MaximumSize;
        Anchor = AnchorStyles.Right | AnchorStyles.Top;
    }

    [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
    private static extern IntPtr CreateRoundRectRgn
        (
        int nLeftRect, // x-coordinate of upper-left corner
        int nTopRect, // y-coordinate of upper-left corner
        int nRightRect, // x-coordinate of lower-right corner
        int nBottomRect, // y-coordinate of lower-right corner
        int nWidthEllipse, // height of ellipse
        int nHeightEllipse // width of ellipse
        );

    protected override void OnClick(EventArgs e)
    {
        FindForm().WindowState = FormWindowState.Minimized;
    }

    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        SetRegion();
        var BigPen = new Pen(Color.FromArgb(161, 161, 161));
        BigPen.Width = 1.8f;
        BigPen.SetLineCap(LineCap.Triangle, LineCap.Triangle, DashCap.Round);
        switch (State)
        {
            case MouseState.None:
                var BGGradientDark = new LinearGradientBrush(new Rectangle(-1, -1, Width, 28),
                    Color.FromArgb(46, 46, 46), Color.FromArgb(33, 33, 33), 90, false);
                BGGradientDark.SetSigmaBellShape(1f, 0.80f);
                G.FillRectangle(BGGradientDark, new Rectangle(-1, -1, Width + 1, 28));
                G.DrawLine(BigPen, 3, 12, 14, 12);
                break;
            case MouseState.Over:
                G.Clear(Color.FromArgb(75, 75, 75));
                G.DrawLine(BigPen, 3, 12, 14, 12);
                break;
        }
    }

    private void SetRegion()
    {
        Region = GetRoundedRegion(Width, Height);
        G.InterpolationMode = InterpolationMode.HighQualityBilinear;
        G.CompositingQuality = CompositingQuality.HighQuality;
        G.SmoothingMode = SmoothingMode.AntiAlias;
    }

    private Region GetRoundedRegion(int controlWidth, int controlHeight)
    {
        return Region.FromHrgn(CreateRoundRectRgn(0, 0, controlWidth + 1, controlHeight + 1, 3, 3));
    }
}


public class ExtendedGraphics
{
    public ExtendedGraphics(Graphics graphics)
    {
        Graphics = graphics;
    }

    #region Fills a Rounded Rectangle with integers.

    public void FillRoundRectangle(Brush brush,
        int x, int y,
        int width, int height, int radius)
    {
        float fx = Convert.ToSingle(x);
        float fy = Convert.ToSingle(y);
        float fwidth = Convert.ToSingle(width);
        float fheight = Convert.ToSingle(height);
        float fradius = Convert.ToSingle(radius);
        FillRoundRectangle(brush, fx, fy,
            fwidth, fheight, fradius);
    }

    #endregion

    #region Fills a Rounded Rectangle with continuous numbers.

    public void FillRoundRectangle(Brush brush,
        float x, float y,
        float width, float height, float radius)
    {
        var rectangle = new RectangleF(x, y, width, height);
        GraphicsPath path = GetRoundedRect(rectangle, radius);
        Graphics.FillPath(brush, path);
    }

    #endregion

    #region Draws a Rounded Rectangle border with integers.

    public void DrawRoundRectangle(Pen pen, int x, int y,
        int width, int height, int radius)
    {
        float fx = Convert.ToSingle(x);
        float fy = Convert.ToSingle(y);
        float fwidth = Convert.ToSingle(width);
        float fheight = Convert.ToSingle(height);
        float fradius = Convert.ToSingle(radius);
        DrawRoundRectangle(pen, fx, fy, fwidth, fheight, fradius);
    }

    #endregion

    #region Draws a Rounded Rectangle border with continuous numbers.

    public void DrawRoundRectangle(Pen pen,
        float x, float y,
        float width, float height, float radius)
    {
        var rectangle = new RectangleF(x, y, width, height);
        GraphicsPath path = GetRoundedRect(rectangle, radius);
        Graphics.DrawPath(pen, path);
    }

    #endregion

    #region Get the desired Rounded Rectangle path.

    public GraphicsPath GetRoundedRect(RectangleF baseRect,
        float radius)
    {
        // if corner radius is less than or equal to zero, 
        // return the original rectangle 
        if (radius <= 0.0F)
        {
            var mPath = new GraphicsPath();
            mPath.AddRectangle(baseRect);
            mPath.CloseFigure();
            return mPath;
        }

        // if the corner radius is greater than or equal to 
        // half the width, or height (whichever is shorter) 
        // then return a capsule instead of a lozenge 
        if (radius >= (Math.Min(baseRect.Width, baseRect.Height)) / 2.0)
            return GetCapsule(baseRect);

        // create the arc for the rectangle sides and declare 
        // a graphics path object for the drawing 
        float diameter = radius * 2.0F;
        var sizeF = new SizeF(diameter, diameter);
        var arc = new RectangleF(baseRect.Location, sizeF);
        var path = new GraphicsPath();

        // top left arc 
        path.AddArc(arc, 180, 90);

        // top right arc 
        arc.X = baseRect.Right - diameter;
        path.AddArc(arc, 270, 90);

        // bottom right arc 
        arc.Y = baseRect.Bottom - diameter;
        path.AddArc(arc, 0, 90);

        // bottom left arc
        arc.X = baseRect.Left;
        path.AddArc(arc, 90, 90);

        path.CloseFigure();
        return path;
    }

    #endregion

    #region Gets the desired Capsular path.

    private GraphicsPath GetCapsule(RectangleF baseRect)
    {
        float diameter;
        RectangleF arc;
        var path = new GraphicsPath();
        try
        {
            if (baseRect.Width > baseRect.Height)
            {
                // return horizontal capsule 
                diameter = baseRect.Height;
                var sizeF = new SizeF(diameter, diameter);
                arc = new RectangleF(baseRect.Location, sizeF);
                path.AddArc(arc, 90, 180);
                arc.X = baseRect.Right - diameter;
                path.AddArc(arc, 270, 180);
            }
            else if (baseRect.Width < baseRect.Height)
            {
                // return vertical capsule 
                diameter = baseRect.Width;
                var sizeF = new SizeF(diameter, diameter);
                arc = new RectangleF(baseRect.Location, sizeF);
                path.AddArc(arc, 180, 180);
                arc.Y = baseRect.Bottom - diameter;
                path.AddArc(arc, 0, 180);
            }
            else
            {
                // return circle 
                path.AddEllipse(baseRect);
            }
        }
        catch
        {
            path.AddEllipse(baseRect);
        }
        finally
        {
            path.CloseFigure();
        }
        return path;
    }

    #endregion

    public Graphics Graphics { get; set; }
}

#endregion

#region modern

//Modern Theme
public class MTheme : Control
{
    private readonly Color C1 = Color.FromArgb(74, 74, 74);
    private readonly Color C3 = Color.FromArgb(41, 41, 41);
    private readonly Color C4 = Color.FromArgb(27, 27, 27);
    private readonly Color C5 = Color.FromArgb(0, 0, 0, 0);
    private readonly Color C6 = Color.FromArgb(25, 255, 255, 255);
    private Color C2 = Color.FromArgb(63, 63, 63);
    private HorizontalAlignment _TitleAlign = (HorizontalAlignment)2;
    private int _TitleHeight = 25;

    public int TitleHeight
    {
        get { return _TitleHeight; }
        set
        {
            if (value > Height)
            {
                value = Height;
            }
            if (value < 2)
            {
                Height = 1;
            }
            _TitleHeight = value;
            Invalidate();
        }
    }

    public HorizontalAlignment TitleAlign
    {
        get { return _TitleAlign; }
        set
        {
            _TitleAlign = value;
            Invalidate();
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        Dock = (DockStyle)5;
        if (Parent is Form)
        {
            ((Form)Parent).FormBorderStyle = 0;
        }
        base.OnHandleCreated(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (
            new Rectangle(Parent.Location.X, Parent.Location.Y, Width - 1, _TitleHeight - 1).IntersectsWith(
                new Rectangle(MousePosition.X, MousePosition.Y, 1, 1)))
        {
            Capture = false;
            Message M = Message.Create(Parent.Handle, 161, new IntPtr(2), new IntPtr(0));
            DefWndProc(ref M);
        }
        base.OnMouseDown(e);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        using (var B = new Bitmap(Width, Height))
        {
            using (Graphics G = Graphics.FromImage(B))
            {
                G.Clear(C3);

                Draw.Gradient(G, C4, C3, 0, 0, Width, _TitleHeight);

                SizeF S = G.MeasureString(Text, Font);
                int O = 6;
                if ((int)_TitleAlign == 2)
                {
                    O = Width / 2 - (int)S.Width / 2;
                }
                if ((int)_TitleAlign == 1)
                {
                    O = Width - (int)S.Width - 6;
                }
                var R = new Rectangle(O, (_TitleHeight + 2) / 2 - (int)S.Height / 2, (int)S.Width, (int)S.Height);
                using (var T = new LinearGradientBrush(R, C1, C3, LinearGradientMode.Vertical))
                {
                    G.DrawString(Text, Font, T, R);
                }

                //  DrawLine(new Pen(C3), 0, 1, Width, 1);

                Draw.Blend(G, C5, C6, C5, 0.5F, 0, 0, _TitleHeight + 1, Width, 1);

                G.DrawLine(new Pen(C4), 0, _TitleHeight, Width, _TitleHeight);
                G.DrawRectangle(new Pen(C4), 0, 0, Width - 1, Height - 1);

                e.Graphics.DrawImage((Image)B.Clone(), 0, (float)0);
            }
        }
    }
}

public class MButton : Control
{
    private readonly Color C1 = Color.FromArgb(31, 31, 31);
    private readonly Color C2 = Color.FromArgb(41, 41, 41);
    private readonly Color C3 = Color.FromArgb(51, 51, 51);
    private readonly Color C4 = Color.FromArgb(0, 0, 0, 0);
    private readonly Color C5 = Color.FromArgb(25, 255, 255, 255);
    private int State;

    public MButton()
    {
        ForeColor = Color.FromArgb(65, 65, 65);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        State = 1;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        State = 2;
        Invalidate();
        base.OnMouseDown(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        State = 0;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        State = 1;
        Invalidate();
        base.OnMouseUp(e);
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        using (var B = new Bitmap(Width, Height))
        {
            using (Graphics G = Graphics.FromImage(B))
            {
                G.DrawRectangle(new Pen(C1), 0, 0, Width - 1, Height - 1);

                if (State == 2)
                {
                    Draw.Gradient(G, C2, C3, 1, 1, Width - 2, Height - 2);
                }
                else
                {
                    Draw.Gradient(G, C3, C2, 1, 1, Width - 2, Height - 2);
                }

                SizeF O = G.MeasureString(Text, Font);
                G.DrawString(Text, Font, new SolidBrush(ForeColor), Width / 2 - O.Width / 2, Height / 2 - O.Height / 2);

                Draw.Blend(G, C4, C5, C4, 0.5F, 0, 1, 1, Width - 2, 1);

                e.Graphics.DrawImage((Image)B.Clone(), 0, (float)0);
            }
        }
    }
}

public class MProgress : Control
{
    private readonly Color C1 = Color.FromArgb(31, 31, 31);
    private readonly Color C2 = Color.FromArgb(41, 41, 41);
    private readonly Color C3 = Color.FromArgb(51, 51, 51);
    private Color C4 = Color.FromArgb(0, 0, 0, 0);
    private Color C5 = Color.FromArgb(25, 255, 255, 255);
    private int _Maximum = 100;
    private int _Value;

    public int Value
    {
        get { return _Value; }
        set
        {
            _Value = value;
            Invalidate();
        }
    }

    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value == 0)
            {
                value = 1;
            }
            _Maximum = value;
            Invalidate();
        }
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        int V = Width * _Value / _Maximum;
        using (var B = new Bitmap(Width, Height))
        {
            using (Graphics G = Graphics.FromImage(B))
            {
                Draw.Gradient(G, C2, C3, 1, 1, Width - 2, Height - 2);
                G.DrawRectangle(new Pen(C2), 1, 1, V - 3, Height - 3);
                Draw.Gradient(G, C3, C2, 2, 2, V - 4, Height - 4);

                G.DrawRectangle(new Pen(C1), 0, 0, Width - 1, Height - 1);

                e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
            }
        }
    }
}

#endregion

#region Carbo Dark Blue

internal class mCarbonDarkBlueForm : ThemeContainer153
{
    public mCarbonDarkBlueForm()
    {
        Header = 24;
        TransparencyKey = Color.Fuchsia;
    }


    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        G.Clear(Color.FromArgb(24, 24, 24));

        DrawGradient(Color.FromArgb(0, 55, 90), Color.FromArgb(0, 70, 128), 11, 8, Width - 22, 17);
        G.FillRectangle(new SolidBrush(Color.FromArgb(0, 55, 90)), 11, 3, Width - 22, 5);

        var P = new Pen(Color.FromArgb(13, Color.White));
        G.DrawLine(P, 10, 1, 10, Height);
        G.DrawLine(P, Width - 11, 1, Width - 11, Height);
        G.DrawLine(P, 11, Height - 11, Width - 12, Height - 11);
        G.DrawLine(P, 11, 29, Width - 12, 29);
        G.DrawLine(P, 11, 25, Width - 12, 25);

        G.FillRectangle(new SolidBrush(Color.FromArgb(13, Color.White)), 0, 2, Width, 6);
        G.FillRectangle(new SolidBrush(Color.FromArgb(13, Color.White)), 0, Height - 6, Width, 4);

        G.FillRectangle(new SolidBrush(Color.FromArgb(24, 24, 24)), 11, Height - 6, Width - 22, 4);

        var T = new HatchBrush(HatchStyle.Trellis, Color.FromArgb(24, 24, 24), Color.FromArgb(8, 8, 8));
        G.FillRectangle(T, 11, 30, Width - 22, Height - 41);

        DrawText(Brushes.White, HorizontalAlignment.Left, 15, 2);

        DrawBorders(new Pen(Color.FromArgb(58, 58, 58)), 1);
        DrawBorders(Pens.Black);

        P = new Pen(Color.FromArgb(25, Color.White));
        G.DrawLine(P, 11, 3, Width - 12, 3);
        G.DrawLine(P, 12, 2, 12, 7);
        G.DrawLine(P, Width - 13, 2, Width - 13, 7);

        G.DrawLine(Pens.Black, 11, 0, 11, Height);
        G.DrawLine(Pens.Black, Width - 12, 0, Width - 12, Height);

        G.DrawRectangle(Pens.Black, 11, 2, Width - 23, 22);
        G.DrawLine(Pens.Black, 11, Height - 12, Width - 12, Height - 12);
        G.DrawLine(Pens.Black, 11, 30, Width - 12, 30);

        DrawCorners(Color.Fuchsia);
    }
}

internal class mCarbonDarkBlue : ThemeControl153
{
    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        DrawBorders(new Pen(Color.FromArgb(32, 32, 32)), 1);
        G.FillRectangle(new SolidBrush(Color.FromArgb(62, 62, 62)), 0, 0, Width, 8);
        DrawBorders(Pens.Black, 2);
        DrawBorders(Pens.Black);

        if (State == MouseState.Over)
        {
            G.FillRectangle(new SolidBrush(Color.FromArgb(0, 55, 90)), 3, 3, Width - 6, Height - 6);
            DrawBorders(new Pen(Color.FromArgb(0, 66, 108)), 3);
        }
        else if (State == MouseState.Down)
        {
            G.FillRectangle(new SolidBrush(Color.FromArgb(0, 44, 72)), 3, 3, Width - 6, Height - 6);
            DrawBorders(new Pen(Color.FromArgb(0, 55, 90)), 3);
        }
        else
        {
            G.FillRectangle(new SolidBrush(Color.FromArgb(24, 24, 24)), 3, 3, Width - 6, Height - 6);
            DrawBorders(new Pen(Color.FromArgb(38, 38, 38)), 3);
        }

        G.FillRectangle(new SolidBrush(Color.FromArgb(13, Color.White)), 3, 3, Width - 6, 8);

        if (State == MouseState.Down)
        {
            DrawText(Brushes.White, HorizontalAlignment.Center, 1, 1);
        }
        else
        {
            DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0);
        }
    }
}

internal class mCarbonDarkBlueGroupBox : ThemeContainer153
{
    public mCarbonDarkBlueGroupBox()
    {
        ControlMode = true;
        Header = 26;
    }


    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        G.Clear(Color.FromArgb(24, 24, 24));

        DrawGradient(Color.FromArgb(0, 55, 90), Color.FromArgb(0, 70, 128), 5, 5, Width - 10, 26);
        G.DrawLine(new Pen(Color.FromArgb(20, Color.White)), 7, 7, Width - 8, 7);

        DrawBorders(Pens.Black, 5, 5, Width - 10, 26, 1);
        DrawBorders(new Pen(Color.FromArgb(36, 36, 36)), 5, 5, Width - 10, 26);

        //???
        DrawBorders(new Pen(Color.FromArgb(8, 8, 8)), 5, 34, Width - 10, Height - 39, 1);
        DrawBorders(new Pen(Color.FromArgb(36, 36, 36)), 5, 34, Width - 10, Height - 39);

        DrawBorders(new Pen(Color.FromArgb(36, 36, 36)), 1);
        DrawBorders(Pens.Black);

        G.DrawLine(new Pen(Color.FromArgb(48, 48, 48)), 1, 1, Width - 2, 1);

        DrawText(Brushes.White, HorizontalAlignment.Left, 9, 5);
    }
}

internal class mCarbonDarkBlueBackground : ThemeContainer153
{
    public mCarbonDarkBlueBackground()
    {
        TransparencyKey = Color.Fuchsia;
    }


    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        G.Clear(Color.FromArgb(24, 24, 24));
        var P = new Pen(Color.FromArgb(13, Color.White));
        var T = new HatchBrush(HatchStyle.Trellis, Color.FromArgb(24, 24, 24), Color.FromArgb(8, 8, 8));
        G.FillRectangle(T, 0, 0, Width, Height);
    }
}

internal class mCarbonDarkBlueCheckbox : ThemeControl153
{
    private bool _Checked;

    public mCarbonDarkBlueCheckbox()
    {
        Click += DroneCheckbox_Click;
        Transparent = true;
        BackColor = Color.Transparent;
        LockHeight = 16;
    }

    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }

    public void DroneCheckbox_Click(object sender, EventArgs e)
    {
        _Checked = !_Checked;
    }


    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        G.Clear(BackColor);
        DrawBorders(new Pen(Color.FromArgb(32, 32, 32)), 0, 0, 16, 16, 1);
        G.FillRectangle(new SolidBrush(Color.FromArgb(62, 62, 62)), 0, 0, 16, 5);
        DrawBorders(Pens.Black, 0, 0, 16, 16, 2);
        DrawBorders(Pens.Black, 0, 0, 16, 16);

        if (_Checked)
        {
            if (State == MouseState.Over)
            {
                G.FillRectangle(new SolidBrush(Color.FromArgb(0, 55, 90)), 3, 3, 10, 10);
                DrawBorders(new Pen(Color.FromArgb(0, 66, 108)), 0, 0, 16, 16, 3);
            }
            else
            {
                G.FillRectangle(new SolidBrush(Color.FromArgb(0, 44, 72)), 3, 3, 10, 10);
                DrawBorders(new Pen(Color.FromArgb(0, 55, 90)), 0, 0, 16, 16, 3);
            }
        }
        else
        {
            if (State == MouseState.Over)
            {
                G.FillRectangle(new SolidBrush(Color.FromArgb(35, 35, 35)), 3, 3, 10, 10);
                DrawBorders(new Pen(Color.FromArgb(49, 49, 49)), 0, 0, 16, 16, 3);
            }
            else
            {
                G.FillRectangle(new SolidBrush(Color.FromArgb(24, 24, 24)), 3, 3, 10, 10);
                DrawBorders(new Pen(Color.FromArgb(38, 38, 38)), 0, 0, 16, 16, 3);
            }
        }

        G.FillRectangle(new SolidBrush(Color.FromArgb(13, Color.White)), 3, 3, 10, 5);

        DrawText(Brushes.White, HorizontalAlignment.Left, 18, 0);
    }
}

#endregion

#region Carbo Dark Orange

internal class mCarbonDarkOrangeForm : ThemeContainer153
{
    public mCarbonDarkOrangeForm()
    {
        Header = 24;
        TransparencyKey = Color.Fuchsia;
    }


    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        G.Clear(Color.FromArgb(24, 24, 24));

        DrawGradient(Color.FromArgb(148, 111, 0), Color.FromArgb(234, 159, 15), 11, 8, Width - 22, 17);
        G.FillRectangle(new SolidBrush(Color.FromArgb(214, 166, 0)), 11, 3, Width - 22, 5);

        var P = new Pen(Color.FromArgb(13, Color.White));
        G.DrawLine(P, 10, 1, 10, Height);
        G.DrawLine(P, Width - 11, 1, Width - 11, Height);
        G.DrawLine(P, 11, Height - 11, Width - 12, Height - 11);
        G.DrawLine(P, 11, 29, Width - 12, 29);
        G.DrawLine(P, 11, 25, Width - 12, 25);

        G.FillRectangle(new SolidBrush(Color.FromArgb(13, Color.White)), 0, 2, Width, 6);
        G.FillRectangle(new SolidBrush(Color.FromArgb(13, Color.White)), 0, Height - 6, Width, 4);

        G.FillRectangle(new SolidBrush(Color.FromArgb(24, 24, 24)), 11, Height - 6, Width - 22, 4);

        var T = new HatchBrush(HatchStyle.Trellis, Color.FromArgb(24, 24, 24), Color.FromArgb(8, 8, 8));
        G.FillRectangle(T, 11, 30, Width - 22, Height - 41);

        DrawText(Brushes.White, HorizontalAlignment.Left, 15, 2);

        DrawBorders(new Pen(Color.FromArgb(58, 58, 58)), 1);
        DrawBorders(Pens.Black);

        P = new Pen(Color.FromArgb(25, Color.White));
        G.DrawLine(P, 11, 3, Width - 12, 3);
        G.DrawLine(P, 12, 2, 12, 7);
        G.DrawLine(P, Width - 13, 2, Width - 13, 7);

        G.DrawLine(Pens.Black, 11, 0, 11, Height);
        G.DrawLine(Pens.Black, Width - 12, 0, Width - 12, Height);

        G.DrawRectangle(Pens.Black, 11, 2, Width - 23, 22);
        G.DrawLine(Pens.Black, 11, Height - 12, Width - 12, Height - 12);
        G.DrawLine(Pens.Black, 11, 30, Width - 12, 30);

        DrawCorners(Color.Fuchsia);
    }
}

internal class mCarbonDarkOrange : ThemeControl153
{
    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        DrawBorders(new Pen(Color.FromArgb(32, 32, 32)), 1);
        G.FillRectangle(new SolidBrush(Color.FromArgb(62, 62, 62)), 0, 0, Width, 8);
        DrawBorders(Pens.Black, 2);
        DrawBorders(Pens.Black);

        if (State == MouseState.Over)
        {
            G.FillRectangle(new SolidBrush(Color.FromArgb(0, 55, 90)), 3, 3, Width - 6, Height - 6);
            DrawBorders(new Pen(Color.FromArgb(0, 66, 108)), 3);
        }
        else if (State == MouseState.Down)
        {
            G.FillRectangle(new SolidBrush(Color.FromArgb(0, 44, 72)), 3, 3, Width - 6, Height - 6);
            DrawBorders(new Pen(Color.FromArgb(0, 55, 90)), 3);
        }
        else
        {
            G.FillRectangle(new SolidBrush(Color.FromArgb(24, 24, 24)), 3, 3, Width - 6, Height - 6);
            DrawBorders(new Pen(Color.FromArgb(38, 38, 38)), 3);
        }

        G.FillRectangle(new SolidBrush(Color.FromArgb(13, Color.White)), 3, 3, Width - 6, 8);

        if (State == MouseState.Down)
        {
            DrawText(Brushes.White, HorizontalAlignment.Center, 1, 1);
        }
        else
        {
            DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0);
        }
    }
}


internal class mCarbonDarkOrangeGroupBox : ThemeContainer153
{
    public mCarbonDarkOrangeGroupBox()
    {
        ControlMode = true;
        Header = 26;
    }


    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        G.Clear(Color.FromArgb(24, 24, 24));

        DrawGradient(Color.OrangeRed, Color.Yellow, 5, 5, Width - 10, 26);
        G.DrawLine(new Pen(Color.FromArgb(20, Color.White)), 7, 7, Width - 8, 7);

        DrawBorders(Pens.Black, 5, 5, Width - 10, 26, 1);
        DrawBorders(new Pen(Color.FromArgb(36, 36, 36)), 5, 5, Width - 10, 26);

        //???
        DrawBorders(new Pen(Color.FromArgb(8, 8, 8)), 5, 34, Width - 10, Height - 39, 1);
        DrawBorders(new Pen(Color.FromArgb(36, 36, 36)), 5, 34, Width - 10, Height - 39);

        DrawBorders(new Pen(Color.FromArgb(36, 36, 36)), 1);
        DrawBorders(Pens.Black);

        G.DrawLine(new Pen(Color.FromArgb(48, 48, 48)), 1, 1, Width - 2, 1);

        DrawText(Brushes.White, HorizontalAlignment.Left, 9, 5);
    }
}

internal class mCarbonDarkOrangeBackground : ThemeContainer153
{
    public mCarbonDarkOrangeBackground()
    {
        TransparencyKey = Color.Fuchsia;
    }


    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        G.Clear(Color.FromArgb(24, 24, 24));
        var P = new Pen(Color.FromArgb(13, Color.White));
        var T = new HatchBrush(HatchStyle.Trellis, Color.FromArgb(24, 24, 24), Color.FromArgb(8, 8, 8));
        G.FillRectangle(T, 0, 0, Width, Height);
    }
}

internal class mCarbonDarkOrangeCheckbox : ThemeControl153
{
    private bool _Checked;

    public mCarbonDarkOrangeCheckbox()
    {
        Click += DroneCheckbox_Click;
        Transparent = true;
        BackColor = Color.Transparent;
        LockHeight = 16;
    }

    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }

    public void DroneCheckbox_Click(object sender, EventArgs e)
    {
        _Checked = !_Checked;
    }


    protected override void ColorHook()
    {
    }

    protected override void PaintHook()
    {
        G.Clear(BackColor);
        DrawBorders(new Pen(Color.FromArgb(32, 32, 32)), 0, 0, 16, 16, 1);
        G.FillRectangle(new SolidBrush(Color.FromArgb(62, 62, 62)), 0, 0, 16, 5);
        DrawBorders(Pens.Black, 0, 0, 16, 16, 2);
        DrawBorders(Pens.Black, 0, 0, 16, 16);

        if (_Checked)
        {
            if (State == MouseState.Over)
            {
                G.FillRectangle(new SolidBrush(Color.FromArgb(0, 55, 90)), 3, 3, 10, 10);
                DrawBorders(new Pen(Color.FromArgb(0, 66, 108)), 0, 0, 16, 16, 3);
            }
            else
            {
                G.FillRectangle(new SolidBrush(Color.FromArgb(0, 44, 72)), 3, 3, 10, 10);
                DrawBorders(new Pen(Color.FromArgb(0, 55, 90)), 0, 0, 16, 16, 3);
            }
        }
        else
        {
            if (State == MouseState.Over)
            {
                G.FillRectangle(new SolidBrush(Color.FromArgb(35, 35, 35)), 3, 3, 10, 10);
                DrawBorders(new Pen(Color.FromArgb(49, 49, 49)), 0, 0, 16, 16, 3);
            }
            else
            {
                G.FillRectangle(new SolidBrush(Color.FromArgb(24, 24, 24)), 3, 3, 10, 10);
                DrawBorders(new Pen(Color.FromArgb(38, 38, 38)), 0, 0, 16, 16, 3);
            }
        }

        G.FillRectangle(new SolidBrush(Color.FromArgb(13, Color.White)), 3, 3, 10, 5);

        DrawText(Brushes.White, HorizontalAlignment.Left, 18, 0);
    }
}

#endregion

#region Unformed

public class mUnformedGiftyTheme : ContainerControl
{
    private Point MouseP = new Point(0, 0);
    private bool cap;
    private int moveheight = 29;

    public mUnformedGiftyTheme()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.FromArgb(25, 25, 25);
        DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        var TopLeft = new Rectangle(0, 0, Width - 125, 28);
        var TopRight = new Rectangle(Width - 82, 0, 81, 28);
        var Body = new Rectangle(10, 10, Width - 21, Height - 16);
        var Body2 = new Rectangle(5, 5, Width - 11, Height - 6);
        base.OnPaint(e);
        var BodyBrush = new LinearGradientBrush(Body2, Color.FromArgb(25, 25, 25), Color.FromArgb(30, 35, 48), 90);
        var BodyBrush2 = new LinearGradientBrush(Body, Color.FromArgb(46, 46, 46), Color.FromArgb(50, 55, 58), 120);
        var gloss = new LinearGradientBrush(new Rectangle(0, 0, Width - 128, 28 / 2),
            Color.FromArgb(240, Color.FromArgb(26, 26, 26)), Color.FromArgb(5, 255, 255, 255), 90);
        var gloss2 = new LinearGradientBrush(new Rectangle(Width - 82, 0, Width - 205, 28 / 2),
            Color.FromArgb(240, Color.FromArgb(26, 26, 26)), Color.FromArgb(5, 255, 255, 255), 90);
        var mainbrush = new LinearGradientBrush(TopLeft, Color.FromArgb(26, 26, 26), Color.FromArgb(30, 30, 30), 90);
        var mainbrush2 = new LinearGradientBrush(TopRight, Color.FromArgb(26, 26, 26), Color.FromArgb(30, 30, 30), 90);
        var P1 = new Pen(Color.FromArgb(174, 195, 30), 2);
        var drawFont = new Font("Tahoma", 10, FontStyle.Bold);

        G.Clear(Color.Fuchsia);
        G.FillPath(BodyBrush, Draw.RoundRect(Body2, 3));
        G.DrawPath(Pens.Black, Draw.RoundRect(Body2, 3));

        G.FillPath(BodyBrush2, Draw.RoundRect(Body, 3));
        G.DrawPath(Pens.Black, Draw.RoundRect(Body, 3));

        G.FillPath(mainbrush, Draw.RoundRect(TopLeft, 3));
        G.FillPath(gloss, Draw.RoundRect(TopLeft, 3));
        G.DrawPath(Pens.Black, Draw.RoundRect(TopLeft, 3));

        G.FillPath(mainbrush, Draw.RoundRect(TopRight, 3));
        G.FillPath(gloss2, Draw.RoundRect(TopRight, 3));
        G.DrawPath(Pens.Black, Draw.RoundRect(TopRight, 3));

        G.DrawLine(P1, 14, 9, 14, 22);
        G.DrawLine(P1, 17, 6, 17, 25);
        G.DrawLine(P1, 20, 9, 20, 22);
        G.DrawLine(P1, 11, 12, 11, 19);
        G.DrawLine(P1, 23, 12, 23, 19);
        G.DrawLine(P1, 8, 14, 8, 17);
        G.DrawLine(P1, 26, 14, 26, 17);
        G.DrawString(Text, drawFont, new SolidBrush(Color.WhiteSmoke), new Rectangle(32, 1, Width - 1, 27),
            new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button == MouseButtons.Left && new Rectangle(0, 0, Width, moveheight).Contains(e.Location))
        {
            cap = true;
            MouseP = e.Location;
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        cap = false;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (cap)
        {
            var p = new Point();
            p.X = MousePosition.X - MouseP.X;
            p.Y = MousePosition.Y - MouseP.Y;
            Parent.Location = p;
        }
    }

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        ParentForm.FormBorderStyle = FormBorderStyle.None;
        ParentForm.TransparencyKey = Color.Fuchsia;
        Dock = DockStyle.Fill;
    }
}

public class mUnformedGiftyButton : Control
{
    private MouseState State = MouseState.None;

    public mUnformedGiftyButton()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(205, 205, 205);
        DoubleBuffered = true;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        State = MouseState.Down;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        State = MouseState.Over;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        State = MouseState.Over;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        State = MouseState.None;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        var ClientRectangle = new Rectangle(0, 0, Width - 1, Height - 1);
        base.OnPaint(e);
        G.Clear(BackColor);
        var drawFont = new Font("Tahoma", 8, FontStyle.Bold);
        G.SmoothingMode = SmoothingMode.HighQuality;
        var R1 = new Rectangle(0, 0, Width - 125, 35 / 2);
        var R2 = new Rectangle(5, Height - 10, Width - 11, 5);
        var R3 = new Rectangle(6, Height - 9, Width - 13, 3);
        var R4 = new Rectangle(1, 1, Width - 3, Height - 3);
        var R5 = new Rectangle(1, 0, Width - 1, Height - 1);
        var R6 = new Rectangle(0, -1, Width - 1, Height - 1);
        var lgb = new LinearGradientBrush(ClientRectangle, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90);
        var botbar = new LinearGradientBrush(R2, Color.FromArgb(44, 45, 49), Color.FromArgb(45, 46, 50), 90);
        var fill = new LinearGradientBrush(R3, Color.FromArgb(174, 195, 30), Color.FromArgb(141, 153, 16), 90);
        LinearGradientBrush gloss = null;
        var o = new Pen(Color.FromArgb(50, 50, 50), 1);
        var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        if (State == MouseState.Over)
            gloss = new LinearGradientBrush(R1, Color.FromArgb(15, Color.FromArgb(26, 26, 26)),
                Color.FromArgb(1, 255, 255, 255), 90);
        else if (State == MouseState.Down)
            gloss = new LinearGradientBrush(R1, Color.FromArgb(100, Color.FromArgb(26, 26, 26)),
                Color.FromArgb(1, 255, 255, 255), 90);
        else
            gloss = new LinearGradientBrush(R1, Color.FromArgb(75, Color.FromArgb(26, 26, 26)),
                Color.FromArgb(3, 255, 255, 255), 90);

        G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 2));
        G.FillPath(gloss, Draw.RoundRect(ClientRectangle, 2));
        G.FillPath(botbar, Draw.RoundRect(R2, 1));
        G.FillPath(fill, Draw.RoundRect(R3, 1));
        G.DrawPath(o, Draw.RoundRect(ClientRectangle, 2));
        G.DrawPath(Pens.Black, Draw.RoundRect(R4, 2));
        G.DrawString(Text, drawFont, new SolidBrush(Color.FromArgb(5, 5, 5)), R5, format);
        G.DrawString(Text, drawFont, new SolidBrush(Color.FromArgb(205, 205, 205)), R6, format);

        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}

public class mUnformedGiftyControlBox : Control
{
    private readonly Rectangle MaxBtn = new Rectangle(25, 0, 20, 20);
    private readonly Rectangle MinBtn = new Rectangle(0, 0, 20, 20);
    private MouseState State = MouseState.None;
    private int x = 0;

    public mUnformedGiftyControlBox()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(205, 205, 205);
        DoubleBuffered = true;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Location.X > 0 && e.Location.X < 20)
            FindForm().WindowState = FormWindowState.Minimized;
        else if (e.Location.X > 25 && e.Location.X < 45)
            FindForm().Close();
        State = MouseState.Down;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        State = MouseState.Over;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        State = MouseState.Over;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        State = MouseState.None;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        base.OnPaint(e);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;

        LinearGradientBrush mlgb = null;
        var mf = new Font("Marlett", 9);
        var mfb = new SolidBrush(Color.FromArgb(174, 195, 30));
        var P1 = new Pen(Color.FromArgb(21, 21, 21), 1);
        Color C1 = Color.FromArgb(66, 67, 70);
        Color C2 = Color.FromArgb(43, 44, 48);
        GraphicsPath GP1 = Draw.RoundRect(MinBtn, 4);
        GraphicsPath GP2 = Draw.RoundRect(MaxBtn, 4);
        switch (State)
        {
            case MouseState.None:
                mlgb = new LinearGradientBrush(MinBtn, C1, C2, 90);
                G.FillPath(mlgb, GP1);
                G.DrawPath(P1, GP1);
                G.DrawString("0", mf, mfb, 4, 4);

                G.FillPath(mlgb, GP2);
                G.DrawPath(P1, GP2);
                G.DrawString("r", mf, mfb, 28, 4);
                break;
            case MouseState.Over:
                if (x > 0 && x < 20)
                {
                    mlgb = new LinearGradientBrush(MinBtn, Color.FromArgb(100, C1), Color.FromArgb(100, C2), 90);
                    G.FillPath(mlgb, GP1);
                    G.DrawPath(P1, GP1);
                    G.DrawString("0", mf, mfb, 4, 4);

                    mlgb = new LinearGradientBrush(MaxBtn, C1, C2, 90);
                    G.FillPath(mlgb, Draw.RoundRect(MaxBtn, 4));
                    G.DrawPath(P1, GP2);
                    G.DrawString("r", mf, mfb, 4, 4);
                }
                else if (x > 25 && x < 45)
                {
                    mlgb = new LinearGradientBrush(MinBtn, C1, C2, 90);
                    G.FillPath(mlgb, GP1);
                    G.DrawPath(P1, GP1);
                    G.DrawString("0", mf, mfb, 4, 4);
                    mlgb = new LinearGradientBrush(MaxBtn, Color.FromArgb(100, C1), Color.FromArgb(100, C2), 90);
                    G.FillPath(mlgb, GP2);
                    G.DrawPath(P1, GP2);
                    G.DrawString("r", mf, mfb, 28, 4);
                }
                else
                {
                    mlgb = new LinearGradientBrush(MinBtn, C1, C2, 90);
                    G.FillPath(mlgb, GP1);
                    G.DrawPath(P1, GP1);
                    G.DrawString("0", mf, mfb, 4, 4);

                    var lgb = new LinearGradientBrush(MaxBtn, C1, C2, 90);
                    G.FillPath(lgb, GP2);
                    G.DrawPath(P1, GP2);
                    G.DrawString("r", mf, mfb, 28, 4);
                }
                break;
            case MouseState.Down:
                mlgb = new LinearGradientBrush(MinBtn, C1, C2, 90);
                G.FillPath(mlgb, GP1);
                G.DrawPath(P1, GP1);
                G.DrawString("0", mf, mfb, 4, 4);

                mlgb = new LinearGradientBrush(MaxBtn, C1, C2, 90);
                G.FillPath(mlgb, GP2);
                G.DrawPath(P1, GP2);
                G.DrawString("r", mf, mfb, 28, 4);
                break;
            default:
                mlgb = new LinearGradientBrush(MinBtn, C1, C2, 90);
                G.FillPath(mlgb, GP1);
                G.DrawPath(P1, GP1);
                G.DrawString("0", mf, mfb, 4, 4);

                mlgb = new LinearGradientBrush(MaxBtn, C1, C2, 90);
                G.FillPath(mlgb, GP2);
                G.DrawPath(P1, GP2);
                G.DrawString("r", mf, mfb, 28, 4);
                break;
        }
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}

public class mUnformedGiftyGroupBox : ContainerControl
{
    public mUnformedGiftyGroupBox()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
        DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        var Body = new Rectangle(4, 25, Width - 9, Height - 30);
        var Body2 = new Rectangle(0, 0, Width - 1, Height - 1);
        base.OnPaint(e);
        G.Clear(Color.Transparent);
        G.SmoothingMode = SmoothingMode.HighQuality;
        G.CompositingQuality = CompositingQuality.HighQuality;

        var P1 = new Pen(Color.Black);
        var BodyBrush = new LinearGradientBrush(Body2, Color.FromArgb(26, 26, 26), Color.FromArgb(30, 30, 30), 90);
        var BodyBrush2 = new LinearGradientBrush(Body, Color.FromArgb(46, 46, 46), Color.FromArgb(50, 55, 58), 120);
        var drawFont = new Font("Tahoma", 9, FontStyle.Bold);
        G.FillPath(BodyBrush, Draw.RoundRect(Body2, 3));
        G.DrawPath(P1, Draw.RoundRect(Body2, 3));

        G.FillPath(BodyBrush2, Draw.RoundRect(Body, 3));
        G.DrawPath(P1, Draw.RoundRect(Body, 3));

        G.DrawString(Text, drawFont, new SolidBrush(Color.WhiteSmoke), 67, 14,
            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}

public class mUnformedGiftyProgressBar : Control
{
    private int _Maximum = 100;
    private bool _ShowPercentage;

    private int _Value;

    public mUnformedGiftyProgressBar()
    {
        DoubleBuffered = true;
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
    }

    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            _Maximum = value;
            Invalidate();
        }
    }

    public int Value
    {
        get
        {
            if (_Value == 0)
                return 0;
            return _Value;
        }
        set
        {
            _Value = value;
            if (_Value > _Maximum)
                _Value = _Maximum;
            Invalidate();
        }
    }

    public bool ShowPercentage
    {
        get { return _ShowPercentage; }
        set
        {
            _ShowPercentage = value;
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);

        G.SmoothingMode = SmoothingMode.HighQuality;

        double val = (double)_Value / _Maximum;
        int intValue = Convert.ToInt32(val * Width);
        G.Clear(BackColor);
        Color C1 = Color.FromArgb(174, 195, 30);
        Color C2 = Color.FromArgb(141, 153, 16);
        var R1 = new Rectangle(0, 0, Width - 1, Height - 1);
        var R2 = new Rectangle(0, 0, intValue - 1, Height - 1);
        var R3 = new Rectangle(0, 0, intValue - 1, Height - 2);
        GraphicsPath GP1 = Draw.RoundRect(R1, 1);
        GraphicsPath GP2 = Draw.RoundRect(R2, 2);
        GraphicsPath GP3 = Draw.RoundRect(R3, 1);
        var gB = new LinearGradientBrush(R1, Color.FromArgb(26, 26, 26), Color.FromArgb(30, 30, 30), 90);
        var g1 = new LinearGradientBrush(new Rectangle(2, 2, intValue - 1, Height - 2), C1, C2, 90);
        var h1 = new HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(50, C1), Color.FromArgb(25, C2));
        var P1 = new Pen(Color.Black);

        G.FillPath(gB, GP1);
        G.FillPath(g1, GP3);
        G.FillPath(h1, GP3);
        G.DrawPath(P1, GP1);
        G.DrawPath(new Pen(Color.FromArgb(150, 97, 94, 90)), GP2);
        G.DrawPath(P1, GP2);

        if (_ShowPercentage)
            G.DrawString(Convert.ToString(string.Concat(Value, "%")), Font, Brushes.White, R1,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}

[DefaultEvent("CheckedChanged")]
public class mUnformedGiftyCheckBox : Control
{
    private MouseState State = MouseState.None;
    private bool _Checked;

    public mUnformedGiftyCheckBox()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
        ForeColor = Color.Black;
        Size = new Size(145, 16);
        DoubleBuffered = true;
    }

    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            if (CheckedChanged != null)
                CheckedChanged(this, EventArgs.Empty);
            Invalidate();
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        State = MouseState.Down;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        State = MouseState.Over;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        State = MouseState.Over;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        State = MouseState.None;
        Invalidate();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        Height = 16;
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }

    protected override void OnClick(EventArgs e)
    {
        _Checked = !_Checked;
        if (CheckedChanged != null)
            CheckedChanged(this, EventArgs.Empty);
        base.OnClick(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        G.SmoothingMode = SmoothingMode.HighQuality;
        G.CompositingQuality = CompositingQuality.HighQuality;
        var checkBoxRectangle = new Rectangle(0, 0, Height - 1, Height - 1);
        var bodyGrad = new LinearGradientBrush(checkBoxRectangle, Color.FromArgb(174, 195, 30),
            Color.FromArgb(141, 153, 16), 90);
        var nb = new SolidBrush(Color.FromArgb(205, 205, 205));
        var format = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
        var drawFont = new Font("Tahoma", 9, FontStyle.Bold);
        G.Clear(BackColor);
        G.FillRectangle(bodyGrad, bodyGrad.Rectangle);
        G.DrawRectangle(new Pen(Color.Black), checkBoxRectangle);
        G.DrawString(Text, drawFont, Brushes.Black, new Point(17, 9), format);
        G.DrawString(Text, drawFont, nb, new Point(16, 8), format);

        if (_Checked)
        {
            var chkPoly = new Rectangle(checkBoxRectangle.X + checkBoxRectangle.Width / 4,
                checkBoxRectangle.Y + checkBoxRectangle.Height / 4, checkBoxRectangle.Width / 2, checkBoxRectangle.Height / 2);
            Point[] p =
            {
                new Point(chkPoly.X, chkPoly.Y + chkPoly.Height/2),
                new Point(chkPoly.X + chkPoly.Width/2, chkPoly.Y + chkPoly.Height),
                new Point(chkPoly.X + chkPoly.Width, chkPoly.Y)
            };
            var P1 = new Pen(Color.FromArgb(12, 12, 12), 2);
            for (int i = 0; i <= p.Length - 2; i++)
                G.DrawLine(P1, p[i], p[i + 1]);
        }
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }

    public event EventHandler CheckedChanged;
}

[DefaultEvent("CheckedChanged")]
public class mUnformedGiftyRadioButton : Control
{
    private MouseState State = MouseState.None;
    private bool _Checked;

    public mUnformedGiftyRadioButton()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
        ForeColor = Color.Black;
        Size = new Size(150, 16);
        DoubleBuffered = true;
    }

    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            InvalidateControls();
            if (CheckedChanged != null)
                CheckedChanged(this, EventArgs.Empty);
            Invalidate();
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        State = MouseState.Down;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        State = MouseState.Over;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        State = MouseState.Over;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        State = MouseState.None;
        Invalidate();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        Height = 16;
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        Invalidate();
    }

    protected override void OnClick(EventArgs e)
    {
        Checked = !Checked;
        base.OnClick(e);
    }

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        InvalidateControls();
    }

    private void InvalidateControls()
    {
        if (!IsHandleCreated || !_Checked) return;
        foreach (Control C in Parent.Controls)
            if (C is mUnformedGiftyRadioButton && C != this)
                ((mUnformedGiftyRadioButton)C).Checked = false;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        G.Clear(BackColor);
        var radioBtnRectangle = new Rectangle(0, 0, Height - 1, Height - 1);
        var R1 = new Rectangle(4, 4, Height - 9, Height - 9);
        var format = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
        var bgGrad = new LinearGradientBrush(radioBtnRectangle, Color.FromArgb(174, 195, 30),
            Color.FromArgb(141, 153, 16), 90);
        Color C1 = Color.FromArgb(250, 15, 15, 15);
        var nb = new SolidBrush(Color.FromArgb(205, 205, 205));
        G.SmoothingMode = SmoothingMode.HighQuality;
        G.CompositingQuality = CompositingQuality.HighQuality;
        var drawFont = new Font("Tahoma", 10, FontStyle.Bold);

        G.FillEllipse(bgGrad, radioBtnRectangle);
        G.DrawEllipse(new Pen(Color.Black), radioBtnRectangle);

        if (Checked)
        {
            var chkGrad = new LinearGradientBrush(R1, C1, C1, 90);
            G.FillEllipse(chkGrad, R1);
        }

        G.DrawString(Text, drawFont, Brushes.Black, new Point(17, 2), format);
        G.DrawString(Text, drawFont, nb, new Point(16, 1), format);

        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }

    public event EventHandler CheckedChanged;
}

public class mUnformedGiftyLabel : Control
{
    public mUnformedGiftyLabel()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(205, 205, 205);
        DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        var ClientRectangle = new Rectangle(0, 0, Width - 1, Height - 1);
        base.OnPaint(e);
        G.Clear(BackColor);
        var drawFont = new Font("Tahoma", 9, FontStyle.Bold);
        var format = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
        G.CompositingQuality = CompositingQuality.HighQuality;
        G.SmoothingMode = SmoothingMode.HighQuality;
        G.DrawString(Text, drawFont, new SolidBrush(Color.FromArgb(5, 5, 5)), new Rectangle(1, 0, Width - 1, Height - 1),
            format);
        G.DrawString(Text, drawFont, new SolidBrush(Color.FromArgb(205, 205, 205)),
            new Rectangle(0, -1, Width - 1, Height - 1), format);
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}

#endregion

#region Tab-Controls

internal class DotNetBarTabcontrol : TabControl
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [DefaultValue(false)]
    private bool
        _DrawPointer;

    public DotNetBarTabcontrol()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint |
            ControlStyles.DoubleBuffer, true);
        DoubleBuffered = true;
        SizeMode = TabSizeMode.Fixed;
        ItemSize = new Size(35, 85);
    }

    public bool DrawPointer
    {
        get { return _DrawPointer; }
        set { _DrawPointer = value; }
    }

    public GraphicsPath RoundRect(Rectangle Rectangle, int Curve)
    {
        var P = new GraphicsPath();
        int ArcRectangleWidth = Curve * 2;
        P.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180F, 90F);
        P.AddArc(
            new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth,
                ArcRectangleWidth), -90F, 90F);
        P.AddArc(
            new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X,
                Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0F, 90F);
        P.AddArc(
            new Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth,
                ArcRectangleWidth), 90F, 90F);
        P.AddLine(new Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y),
            new Point(Rectangle.X, Curve + Rectangle.Y));
        return P;
    }

    public GraphicsPath RoundRect(int X, int Y, int Width, int Height, int Curve)
    {
        var Rectangle = new Rectangle(X, Y, Width, Height);
        var P = new GraphicsPath();
        int ArcRectangleWidth = Curve * 2;
        P.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180F, 90F);
        P.AddArc(
            new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth,
                ArcRectangleWidth), -90F, 90F);
        P.AddArc(
            new Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X,
                Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0F, 90F);
        P.AddArc(
            new Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth,
                ArcRectangleWidth), 90F, 90F);
        P.AddLine(new Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y),
            new Point(Rectangle.X, Curve + Rectangle.Y));
        return P;
    }

    protected override void CreateHandle()
    {
        base.CreateHandle();
        Alignment = TabAlignment.Left;
    }

    public Pen ToPen(Color color)
    {
        return new Pen(color);
    }

    public Brush ToBrush(Color color)
    {
        return new SolidBrush(color);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        try
        {
            SelectedTab.BackColor = Color.FromArgb(24, 24, 24);
        }
        catch
        {
        }


        for (int i = 0; i < TabCount; i++)
        {
            if (i == SelectedIndex)
            {
                G.Clear(Color.FromArgb(24, 24, 24));
                var P = new Pen(Color.FromArgb(13, Color.FromArgb(24, 24, 24)));
                var T = new HatchBrush(HatchStyle.Trellis, Color.FromArgb(24, 24, 24), Color.FromArgb(8, 8, 8));
                G.FillRectangle(T, 0, 0, Width, Height);
                var x2 = new Rectangle(new Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2),
                    new Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1));
                var myBlend = new ColorBlend();
                myBlend.Colors = new[] { Color.FromArgb(214, 166, 0), Color.FromArgb(33, 33, 33), Color.FromArgb(96, 110, 121) };
                myBlend.Positions = new[] { 0.0F, 0.5F, 1.0F };
                var lgBrush = new LinearGradientBrush(x2, Color.Black, Color.Black, 90.0F);
                lgBrush.InterpolationColors = myBlend;
                G.FillRectangle(T, x2);
                G.DrawRectangle(P, x2);
                var tabRect = new Rectangle(GetTabRect(i).Location.X + 4, GetTabRect(i).Location.Y + 2,
                    GetTabRect(i).Size.Width + 10, GetTabRect(i).Size.Height - 11);
                G.FillPath(new SolidBrush(Color.FromArgb(80, 90, 100)), RoundRect(tabRect, 5));
                G.DrawPath(new Pen(Color.FromArgb(214, 166, 0)),
                    RoundRect(new Rectangle(tabRect.X + 1, tabRect.Y + 1, tabRect.Width - 1, tabRect.Height - 2), 5));
                G.DrawPath(new Pen(Color.FromArgb(115, 125, 135)), RoundRect(tabRect, 5));

                G.SmoothingMode = SmoothingMode.HighQuality;

                if (_DrawPointer)
                {
                    Point[] p =
                    {
                        new Point(ItemSize.Height - 3, GetTabRect(i).Location.Y + 20),
                        new Point(ItemSize.Height + 4, GetTabRect(i).Location.Y + 14),
                        new Point(ItemSize.Height + 4, GetTabRect(i).Location.Y + 27)
                    };
                    G.FillPolygon(Brushes.Tan, p);
                    Invalidate();
                }

                if (ImageList != null)
                {
                    try
                    {
                        if (ImageList.Images[TabPages[i].ImageIndex] != null)
                        {
                            G.DrawImage(ImageList.Images[TabPages[i].ImageIndex],
                                new Point(x2.Location.X + 8, x2.Location.Y + 6));
                            G.DrawString("      " + TabPages[i].Text.ToUpper(),
                                new Font(Font.FontFamily, Font.Size, FontStyle.Bold), Brushes.White, x2,
                                new StringFormat
                                {
                                    LineAlignment = StringAlignment.Center,
                                    Alignment = StringAlignment.Center
                                });
                        }
                        else
                        {
                            G.DrawString(TabPages[i].Text.ToUpper(),
                                new Font(Font.FontFamily, Font.Size, FontStyle.Bold), Brushes.White, x2,
                                new StringFormat
                                {
                                    LineAlignment = StringAlignment.Center,
                                    Alignment = StringAlignment.Center
                                });
                        }
                    }
                    catch
                    {
                        G.DrawString(TabPages[i].Text.ToUpper(), new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
                            Brushes.White, x2,
                            new StringFormat
                            {
                                LineAlignment = StringAlignment.Center,
                                Alignment = StringAlignment.Center
                            });
                    }
                }
                else
                {
                    G.DrawString(TabPages[i].Text.ToUpper(), new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
                        Brushes.White, x2,
                        new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
                }

                G.DrawLine(new Pen(Color.FromArgb(96, 110, 121)), new Point(x2.Location.X - 1, x2.Location.Y - 1),
                    new Point(x2.Location.X, x2.Location.Y));
                G.DrawLine(new Pen(Color.FromArgb(96, 110, 121)), new Point(x2.Location.X - 1, x2.Bottom - 1),
                    new Point(x2.Location.X, x2.Bottom));
            }
            else
            {
                var x2 = new Rectangle(new Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2),
                    new Size(GetTabRect(i).Width + 3, GetTabRect(i).Height + 1));
                G.FillRectangle(new SolidBrush(Color.FromArgb(96, 110, 121)), x2);
                G.DrawLine(new Pen(Color.FromArgb(96, 110, 121)), new Point(x2.Right, x2.Top),
                    new Point(x2.Right, x2.Bottom));
                if (ImageList != null)
                {
                    try
                    {
                        if (ImageList.Images[TabPages[i].ImageIndex] != null)
                        {
                            G.DrawImage(ImageList.Images[TabPages[i].ImageIndex],
                                new Point(x2.Location.X + 8, x2.Location.Y + 6));
                            G.DrawString("      " + TabPages[i].Text, Font, Brushes.White, x2,
                                new StringFormat
                                {
                                    LineAlignment = StringAlignment.Near,
                                    Alignment = StringAlignment.Near
                                });
                        }
                        else
                        {
                            G.DrawString(TabPages[i].Text.ToUpper(),
                                new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
                                new SolidBrush(Color.FromArgb(210, 220, 230)), x2,
                                new StringFormat
                                {
                                    LineAlignment = StringAlignment.Center,
                                    Alignment = StringAlignment.Center
                                });
                        }
                    }
                    catch
                    {
                        G.DrawString(TabPages[i].Text.ToUpper(), new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
                            new SolidBrush(Color.FromArgb(210, 220, 230)), x2,
                            new StringFormat
                            {
                                LineAlignment = StringAlignment.Center,
                                Alignment = StringAlignment.Center
                            });
                    }
                }
                else
                {
                    G.DrawString(TabPages[i].Text.ToUpper(), new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
                        new SolidBrush(Color.FromArgb(210, 220, 230)), x2,
                        new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
                }
            }
            G.FillPath(Brushes.White, RoundRect(new Rectangle(86, 0, Width - 89, Height - 3), 5));
            G.DrawPath(new Pen(Color.FromArgb(65, 75, 85)), RoundRect(new Rectangle(86, 0, Width - 89, Height - 3), 5));
        }

        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}


internal class Tab_Control_Class : TabControl
{
    public Tab_Control_Class()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint, true);
        DoubleBuffered = true;
        SizeMode = TabSizeMode.Fixed;
        ItemSize = new Size(30, 120);
    }

    protected override void CreateHandle()
    {
        base.CreateHandle();
        Alignment = TabAlignment.Left;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var b = new Bitmap(Width, Height);
        Graphics g = Graphics.FromImage(b);
        g.Clear(Color.Gainsboro);

        for (int i = 0; i <= TabCount - 1; i++)
        {
            Rectangle tabRectangle = GetTabRect(i);
            if (SelectedIndex == i)
            {
                //tab is selected
                g.FillRectangle(Brushes.Red, tabRectangle);
            }
            else
            {
                //tab is not selected
                g.FillRectangle(Brushes.Blue, tabRectangle);
            }

            g.DrawString(TabPages[i].Text, Font, Brushes.White, tabRectangle,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        }
        e.Graphics.DrawImage(b, 0, 0);
        b.Dispose();
        g.Dispose();
        base.OnPaint(e);
    }
}


internal class AnimTabControl : TabControl
{
    private int OldIndex = 1;
    private int Speed = 150;

    public AnimTabControl()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw,
            true);
    }

    protected override void OnDeselected(TabControlEventArgs e)
    {
        OldIndex = e.TabPageIndex;
    }

    protected override void OnSelected(TabControlEventArgs e)
    {
        if (OldIndex < e.TabPageIndex)
            DoAnimationScrollRight(TabPages[OldIndex], TabPages[e.TabPageIndex]);
        else
            DoAnimationScrollLeft(TabPages[OldIndex], TabPages[e.TabPageIndex]);
    }

    private void DoAnimationScrollLeft(Control FirstControl, Control SecondControl)
    {
        Graphics ControlGraphics = FirstControl.CreateGraphics();
        var FirstControlBitmap = new Bitmap(FirstControl.Width, FirstControl.Height);
        var SecondControlBitmap = new Bitmap(SecondControl.Width, SecondControl.Height);

        FirstControl.DrawToBitmap(FirstControlBitmap, new Rectangle(0, 0, FirstControl.Width, FirstControl.Height));
        SecondControl.DrawToBitmap(SecondControlBitmap, new Rectangle(0, 0, SecondControl.Width, SecondControl.Height));

        foreach (Control C in FirstControl.Controls)
            C.Hide();

        int Slide = FirstControl.Width - (FirstControl.Width % Speed);
        int I;

        for (I = 0; I <= Slide; I += Speed)
        {
            ControlGraphics.DrawImage(FirstControlBitmap, new Rectangle(I, 0, FirstControl.Width, FirstControl.Height));
            ControlGraphics.DrawImage(SecondControlBitmap,
                new Rectangle(I - SecondControl.Width, 0, SecondControl.Width, SecondControl.Height));
        }

        I = FirstControl.Width;
        ControlGraphics.DrawImage(FirstControlBitmap, new Rectangle(I, 0, FirstControl.Width, FirstControl.Height));
        ControlGraphics.DrawImage(SecondControlBitmap,
            new Rectangle(I - SecondControl.Width, 0, SecondControl.Width, SecondControl.Height));

        SelectedTab = (TabPage)SecondControl;

        foreach (Control C in FirstControl.Controls)
            C.Show();
    }

    private void DoAnimationScrollRight(Control FirstControl, Control SecondControl)
    {
        Graphics ControlGraphics = FirstControl.CreateGraphics();
        var FirstControlBitmap = new Bitmap(FirstControl.Width, FirstControl.Height);
        var SecondControlBitmap = new Bitmap(SecondControl.Width, SecondControl.Height);

        FirstControl.DrawToBitmap(FirstControlBitmap, new Rectangle(0, 0, FirstControl.Width, FirstControl.Height));
        SecondControl.DrawToBitmap(SecondControlBitmap, new Rectangle(0, 0, SecondControl.Width, SecondControl.Height));

        foreach (Control C in FirstControl.Controls)
            C.Hide();

        int Slide = FirstControl.Width - (FirstControl.Width % Speed);
        int I;

        for (I = 0; I >= -Slide; I -= Speed) // += -Speed
        {
            ControlGraphics.DrawImage(FirstControlBitmap, new Rectangle(I, 0, FirstControl.Width, FirstControl.Height));
            ControlGraphics.DrawImage(SecondControlBitmap,
                new Rectangle(I + SecondControl.Width, 0, SecondControl.Width, SecondControl.Height));
        }

        I = FirstControl.Width;
        ControlGraphics.DrawImage(FirstControlBitmap, new Rectangle(I, 0, FirstControl.Width, FirstControl.Height));
        ControlGraphics.DrawImage(SecondControlBitmap,
            new Rectangle(I + SecondControl.Width, 0, SecondControl.Width, SecondControl.Height));
        SelectedTab = (TabPage)SecondControl;

        foreach (Control C in FirstControl.Controls)
            C.Show();
    }
}

#endregion

#region TUPS

internal class TUPSTheme : ThemeContainer154
{
    // Fields
    private Color Accent;
    private Color Border;
    private Color TextColor;
    private Color TitleBottom;
    private Color TitleTop;
    private bool _ShowIcon;
    // Methods
    public TUPSTheme()
    {
        Header = 30;
        SetColor("Titlebar Gradient Top", 0x3f, 0x3f, 0x3f);
        SetColor("Titlebar Gradient Bottom", 20, 20, 20);
        SetColor("Text", 170, 170, 170);
        SetColor("Accent", 180, 0x1a, 0x20);
        SetColor("Border", Color.Black);
        TransparencyKey = Color.Fuchsia;
        BackColor = Color.FromArgb(30, 30, 30);
        Font = new Font("Segoe UI", 9f);
    }

    public bool ShowIcon
    {
        get { return _ShowIcon; }
        set
        {
            _ShowIcon = value;
            Invalidate();
        }
    }

    protected override void ColorHook()
    {
        TitleTop = GetColor("Titlebar Gradient Top");
        TitleBottom = GetColor("Titlebar Gradient Bottom");
        TextColor = GetColor("Text");
        Accent = GetColor("Accent");
        Border = GetColor("Border");
    }

    protected override void PaintHook()
    {
        base.G.Clear(Border);
        var rect = new Rectangle(1, 1, Width - 2, 0x23);
        var brush = new LinearGradientBrush(rect, TitleTop, TitleBottom, 90f);
        base.G.FillPath(brush, CreateRound(1, 1, Width - 2, 0x23, 7));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), CreateRound(1, 1, Width - 3, 0x23, 7));
        base.G.FillPath(new SolidBrush(BackColor), CreateRound(1, 0x20, Width - 2, Height - 0x21, 7));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), CreateRound(1, 0x20, Width - 3, Height - 0x22, 7));
        rect = new Rectangle(1, 0x20, Width - 2, 3);
        base.G.FillRectangle(new SolidBrush(Border), rect);
        var point = new Point(1, 0x1f);
        var point2 = new Point(Width - 2, 0x1f);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point, point2);
        var blend = new ColorBlend(3);
        blend.Colors = new[] { Color.Black, Accent, Color.Black };
        blend.Positions = new[] { 0f, 0.5f, 1f };
        rect = new Rectangle(1, 0x21, Width - 2, 2);
        DrawGradient(blend, rect, 0f);
        point2 = new Point(1, 0x23);
        point = new Point(Width - 2, 0x23);
        base.G.DrawLine(new Pen(BackColor), point2, point);
        point2 = new Point(1, 0x23);
        point = new Point(Width - 2, 0x23);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point2, point);
        if (_ShowIcon)
        {
            rect = new Rectangle(11, 8, 0x10, 0x10);
            base.G.DrawIcon(FindForm().Icon, rect);
            point2 = new Point(0x20, 8);
            base.G.DrawString(FindForm().Text, Font, new SolidBrush(TextColor), point2);
        }
        else
        {
            point2 = new Point(13, 8);
            base.G.DrawString(FindForm().Text, Font, new SolidBrush(TextColor), point2);
        }
        DrawPixel(Color.Fuchsia, 0, 0);
        DrawPixel(Color.Fuchsia, 1, 0);
        DrawPixel(Color.Fuchsia, 2, 0);
        DrawPixel(Color.Fuchsia, 3, 0);
        DrawPixel(Color.Fuchsia, 0, 1);
        DrawPixel(Color.Fuchsia, 0, 2);
        DrawPixel(Color.Fuchsia, 0, 3);
        DrawPixel(Color.Fuchsia, 1, 1);
        DrawPixel(Color.Fuchsia, Width - 1, 0);
        DrawPixel(Color.Fuchsia, Width - 2, 0);
        DrawPixel(Color.Fuchsia, Width - 3, 0);
        DrawPixel(Color.Fuchsia, Width - 4, 0);
        DrawPixel(Color.Fuchsia, Width - 1, 1);
        DrawPixel(Color.Fuchsia, Width - 1, 2);
        DrawPixel(Color.Fuchsia, Width - 1, 3);
        DrawPixel(Color.Fuchsia, Width - 2, 1);
        DrawPixel(Color.Fuchsia, 0, Height);
        DrawPixel(Color.Fuchsia, 1, Height);
        DrawPixel(Color.Fuchsia, 2, Height);
        DrawPixel(Color.Fuchsia, 3, Height);
        DrawPixel(Color.Fuchsia, 0, Height - 1);
        DrawPixel(Color.Fuchsia, 0, Height - 2);
        DrawPixel(Color.Fuchsia, 0, Height - 3);
        DrawPixel(Color.Fuchsia, 1, Height - 1);
        DrawPixel(Color.Fuchsia, Width - 1, Height);
        DrawPixel(Color.Fuchsia, Width - 2, Height);
        DrawPixel(Color.Fuchsia, Width - 3, Height);
        DrawPixel(Color.Fuchsia, Width - 4, Height);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 1);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 2);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 3);
        DrawPixel(Color.Fuchsia, Width - 2, Height - 1);
    }

    // Properties
}

internal class TUPSThemeX : ThemeContainer154
{
    // Fields
    private Color Accent;
    private Color Border;
    private Color TextColor;
    private Color TitleBottom;
    private Color TitleTop;
    private bool _ShowIcon;
    // Methods
    public TUPSThemeX()
    {
        Header = 30;
        SetColor("Titlebar Gradient Top", 0x3f, 0x3f, 0x3f);
        SetColor("Titlebar Gradient Bottom", 20, 20, 20);
        SetColor("Text", 170, 170, 170);
        SetColor("Accent", 191, 58, 43);
        SetColor("Border", Color.Black);
        TransparencyKey = Color.Fuchsia;
        BackColor = Color.FromArgb(30, 30, 30);
        Font = new Font("Segoe UI", 9f);
    }

    public bool ShowIcon
    {
        get { return _ShowIcon; }
        set
        {
            _ShowIcon = value;
            Invalidate();
        }
    }

    protected override void ColorHook()
    {
        TitleTop = GetColor("Titlebar Gradient Top");
        TitleBottom = GetColor("Titlebar Gradient Bottom");
        TextColor = GetColor("Text");
        Accent = GetColor("Accent");
        Border = GetColor("Border");
    }

    protected override void PaintHook()
    {
        base.G.Clear(Border);
        var rect = new Rectangle(1, 1, Width - 2, 0x23);
        var brush = new LinearGradientBrush(rect, TitleTop, TitleBottom, 90f);
        base.G.FillPath(brush, CreateRound(1, 1, Width - 2, 0x23, 7));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), CreateRound(1, 1, Width - 3, 0x23, 7));
        base.G.FillPath(new SolidBrush(BackColor), CreateRound(1, 0x20, Width - 2, Height - 0x21, 7));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), CreateRound(1, 0x20, Width - 3, Height - 0x22, 7));
        rect = new Rectangle(1, 0x20, Width - 2, 3);
        base.G.FillRectangle(new SolidBrush(Border), rect);
        var point = new Point(1, 0x1f);
        var point2 = new Point(Width - 2, 0x1f);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point, point2);
        var blend = new ColorBlend(3);
        blend.Colors = new[] { Color.Black, Accent, Color.Black };
        blend.Positions = new[] { 0f, 0.5f, 1f };
        rect = new Rectangle(1, 0x21, Width - 2, 2);
        DrawGradient(blend, rect, 0f);
        point2 = new Point(1, 0x23);
        point = new Point(Width - 2, 0x23);
        base.G.DrawLine(new Pen(BackColor), point2, point);
        point2 = new Point(1, 0x23);
        point = new Point(Width - 2, 0x23);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point2, point);
        if (_ShowIcon)
        {
            rect = new Rectangle(11, 8, 0x10, 0x10);
            base.G.DrawIcon(FindForm().Icon, rect);
            point2 = new Point(0x20, 8);
            base.G.DrawString(FindForm().Text, Font, new SolidBrush(TextColor), point2);
        }
        else
        {
            point2 = new Point(13, 8);
            base.G.DrawString(FindForm().Text, Font, new SolidBrush(TextColor), point2);
        }
        DrawPixel(Color.Fuchsia, 0, 0);
        DrawPixel(Color.Fuchsia, 1, 0);
        DrawPixel(Color.Fuchsia, 2, 0);
        DrawPixel(Color.Fuchsia, 3, 0);
        DrawPixel(Color.Fuchsia, 0, 1);
        DrawPixel(Color.Fuchsia, 0, 2);
        DrawPixel(Color.Fuchsia, 0, 3);
        DrawPixel(Color.Fuchsia, 1, 1);
        DrawPixel(Color.Fuchsia, Width - 1, 0);
        DrawPixel(Color.Fuchsia, Width - 2, 0);
        DrawPixel(Color.Fuchsia, Width - 3, 0);
        DrawPixel(Color.Fuchsia, Width - 4, 0);
        DrawPixel(Color.Fuchsia, Width - 1, 1);
        DrawPixel(Color.Fuchsia, Width - 1, 2);
        DrawPixel(Color.Fuchsia, Width - 1, 3);
        DrawPixel(Color.Fuchsia, Width - 2, 1);
        DrawPixel(Color.Fuchsia, 0, Height);
        DrawPixel(Color.Fuchsia, 1, Height);
        DrawPixel(Color.Fuchsia, 2, Height);
        DrawPixel(Color.Fuchsia, 3, Height);
        DrawPixel(Color.Fuchsia, 0, Height - 1);
        DrawPixel(Color.Fuchsia, 0, Height - 2);
        DrawPixel(Color.Fuchsia, 0, Height - 3);
        DrawPixel(Color.Fuchsia, 1, Height - 1);
        DrawPixel(Color.Fuchsia, Width - 1, Height);
        DrawPixel(Color.Fuchsia, Width - 2, Height);
        DrawPixel(Color.Fuchsia, Width - 3, Height);
        DrawPixel(Color.Fuchsia, Width - 4, Height);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 1);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 2);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 3);
        DrawPixel(Color.Fuchsia, Width - 2, Height - 1);
    }

    // Properties
}

internal class TUPSThemeBlue : ThemeContainer154
{
    // Fields
    private Color Accent;
    private Color Border;
    private Color TextColor;
    private Color TitleBottom;
    private Color TitleTop;
    private bool _ShowIcon;
    // Methods
    public TUPSThemeBlue()
    {
        Header = 30;
        SetColor("Titlebar Gradient Top", 0x3f, 0x3f, 0x3f);
        SetColor("Titlebar Gradient Bottom", 20, 20, 20);
        SetColor("Text", 170, 170, 170);
        SetColor("Accent", 24, 123, 190);
        SetColor("Border", Color.Black);
        TransparencyKey = Color.Fuchsia;
        BackColor = Color.FromArgb(30, 30, 30);
        Font = new Font("Segoe UI", 9f);
    }

    public bool ShowIcon
    {
        get { return _ShowIcon; }
        set
        {
            _ShowIcon = value;
            Invalidate();
        }
    }

    protected override void ColorHook()
    {
        TitleTop = GetColor("Titlebar Gradient Top");
        TitleBottom = GetColor("Titlebar Gradient Bottom");
        TextColor = GetColor("Text");
        Accent = GetColor("Accent");
        Border = GetColor("Border");
    }

    protected override void PaintHook()
    {
        base.G.Clear(Border);
        var rect = new Rectangle(1, 1, Width - 2, 0x23);
        var brush = new LinearGradientBrush(rect, TitleTop, TitleBottom, 90f);
        base.G.FillPath(brush, CreateRound(1, 1, Width - 2, 0x23, 7));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), CreateRound(1, 1, Width - 3, 0x23, 7));
        base.G.FillPath(new SolidBrush(BackColor), CreateRound(1, 0x20, Width - 2, Height - 0x21, 7));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), CreateRound(1, 0x20, Width - 3, Height - 0x22, 7));
        rect = new Rectangle(1, 0x20, Width - 2, 3);
        base.G.FillRectangle(new SolidBrush(Border), rect);
        var point = new Point(1, 0x1f);
        var point2 = new Point(Width - 2, 0x1f);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point, point2);
        var blend = new ColorBlend(3);
        blend.Colors = new[] { Color.Black, Accent, Color.Black };
        blend.Positions = new[] { 0f, 0.5f, 1f };
        rect = new Rectangle(1, 0x21, Width - 2, 2);
        DrawGradient(blend, rect, 0f);
        point2 = new Point(1, 0x23);
        point = new Point(Width - 2, 0x23);
        base.G.DrawLine(new Pen(BackColor), point2, point);
        point2 = new Point(1, 0x23);
        point = new Point(Width - 2, 0x23);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point2, point);
        if (_ShowIcon)
        {
            rect = new Rectangle(11, 8, 0x10, 0x10);
            base.G.DrawIcon(FindForm().Icon, rect);
            point2 = new Point(0x20, 8);
            base.G.DrawString(FindForm().Text, Font, new SolidBrush(TextColor), point2);
        }
        else
        {
            point2 = new Point(13, 8);
            base.G.DrawString(FindForm().Text, Font, new SolidBrush(TextColor), point2);
        }
        DrawPixel(Color.Fuchsia, 0, 0);
        DrawPixel(Color.Fuchsia, 1, 0);
        DrawPixel(Color.Fuchsia, 2, 0);
        DrawPixel(Color.Fuchsia, 3, 0);
        DrawPixel(Color.Fuchsia, 0, 1);
        DrawPixel(Color.Fuchsia, 0, 2);
        DrawPixel(Color.Fuchsia, 0, 3);
        DrawPixel(Color.Fuchsia, 1, 1);
        DrawPixel(Color.Fuchsia, Width - 1, 0);
        DrawPixel(Color.Fuchsia, Width - 2, 0);
        DrawPixel(Color.Fuchsia, Width - 3, 0);
        DrawPixel(Color.Fuchsia, Width - 4, 0);
        DrawPixel(Color.Fuchsia, Width - 1, 1);
        DrawPixel(Color.Fuchsia, Width - 1, 2);
        DrawPixel(Color.Fuchsia, Width - 1, 3);
        DrawPixel(Color.Fuchsia, Width - 2, 1);
        DrawPixel(Color.Fuchsia, 0, Height);
        DrawPixel(Color.Fuchsia, 1, Height);
        DrawPixel(Color.Fuchsia, 2, Height);
        DrawPixel(Color.Fuchsia, 3, Height);
        DrawPixel(Color.Fuchsia, 0, Height - 1);
        DrawPixel(Color.Fuchsia, 0, Height - 2);
        DrawPixel(Color.Fuchsia, 0, Height - 3);
        DrawPixel(Color.Fuchsia, 1, Height - 1);
        DrawPixel(Color.Fuchsia, Width - 1, Height);
        DrawPixel(Color.Fuchsia, Width - 2, Height);
        DrawPixel(Color.Fuchsia, Width - 3, Height);
        DrawPixel(Color.Fuchsia, Width - 4, Height);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 1);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 2);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 3);
        DrawPixel(Color.Fuchsia, Width - 2, Height - 1);
    }

    // Properties
}

internal class TUPSThemeGreen : ThemeContainer154
{
    // Fields
    private Color Accent;
    private Color Border;
    private Color TextColor;
    private Color TitleBottom;
    private Color TitleTop;
    private bool _ShowIcon;
    // Methods
    public TUPSThemeGreen()
    {
        Header = 30;
        SetColor("Titlebar Gradient Top", 0x3f, 0x3f, 0x3f);
        SetColor("Titlebar Gradient Bottom", 20, 20, 20);
        SetColor("Text", 170, 170, 170);
        SetColor("Accent", 0, 255, 0);
        SetColor("Border", Color.Black);
        TransparencyKey = Color.Fuchsia;
        BackColor = Color.FromArgb(30, 30, 30);
        Font = new Font("Segoe UI", 9f);
    }

    public bool ShowIcon
    {
        get { return _ShowIcon; }
        set
        {
            _ShowIcon = value;
            Invalidate();
        }
    }

    protected override void ColorHook()
    {
        TitleTop = GetColor("Titlebar Gradient Top");
        TitleBottom = GetColor("Titlebar Gradient Bottom");
        TextColor = GetColor("Text");
        Accent = GetColor("Accent");
        Border = GetColor("Border");
    }

    protected override void PaintHook()
    {
        base.G.Clear(Border);
        var rect = new Rectangle(1, 1, Width - 2, 0x23);
        var brush = new LinearGradientBrush(rect, TitleTop, TitleBottom, 90f);
        base.G.FillPath(brush, CreateRound(1, 1, Width - 2, 0x23, 7));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), CreateRound(1, 1, Width - 3, 0x23, 7));
        base.G.FillPath(new SolidBrush(BackColor), CreateRound(1, 0x20, Width - 2, Height - 0x21, 7));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), CreateRound(1, 0x20, Width - 3, Height - 0x22, 7));
        rect = new Rectangle(1, 0x20, Width - 2, 3);
        base.G.FillRectangle(new SolidBrush(Border), rect);
        var point = new Point(1, 0x1f);
        var point2 = new Point(Width - 2, 0x1f);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point, point2);
        var blend = new ColorBlend(3);
        blend.Colors = new[] { Color.Black, Accent, Color.Black };
        blend.Positions = new[] { 0f, 0.5f, 1f };
        rect = new Rectangle(1, 0x21, Width - 2, 2);
        DrawGradient(blend, rect, 0f);
        point2 = new Point(1, 0x23);
        point = new Point(Width - 2, 0x23);
        base.G.DrawLine(new Pen(BackColor), point2, point);
        point2 = new Point(1, 0x23);
        point = new Point(Width - 2, 0x23);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point2, point);
        if (_ShowIcon)
        {
            rect = new Rectangle(11, 8, 0x10, 0x10);
            base.G.DrawIcon(FindForm().Icon, rect);
            point2 = new Point(0x20, 8);
            base.G.DrawString(FindForm().Text, Font, new SolidBrush(TextColor), point2);
        }
        else
        {
            point2 = new Point(13, 8);
            base.G.DrawString(FindForm().Text, Font, new SolidBrush(TextColor), point2);
        }
        DrawPixel(Color.Fuchsia, 0, 0);
        DrawPixel(Color.Fuchsia, 1, 0);
        DrawPixel(Color.Fuchsia, 2, 0);
        DrawPixel(Color.Fuchsia, 3, 0);
        DrawPixel(Color.Fuchsia, 0, 1);
        DrawPixel(Color.Fuchsia, 0, 2);
        DrawPixel(Color.Fuchsia, 0, 3);
        DrawPixel(Color.Fuchsia, 1, 1);
        DrawPixel(Color.Fuchsia, Width - 1, 0);
        DrawPixel(Color.Fuchsia, Width - 2, 0);
        DrawPixel(Color.Fuchsia, Width - 3, 0);
        DrawPixel(Color.Fuchsia, Width - 4, 0);
        DrawPixel(Color.Fuchsia, Width - 1, 1);
        DrawPixel(Color.Fuchsia, Width - 1, 2);
        DrawPixel(Color.Fuchsia, Width - 1, 3);
        DrawPixel(Color.Fuchsia, Width - 2, 1);
        DrawPixel(Color.Fuchsia, 0, Height);
        DrawPixel(Color.Fuchsia, 1, Height);
        DrawPixel(Color.Fuchsia, 2, Height);
        DrawPixel(Color.Fuchsia, 3, Height);
        DrawPixel(Color.Fuchsia, 0, Height - 1);
        DrawPixel(Color.Fuchsia, 0, Height - 2);
        DrawPixel(Color.Fuchsia, 0, Height - 3);
        DrawPixel(Color.Fuchsia, 1, Height - 1);
        DrawPixel(Color.Fuchsia, Width - 1, Height);
        DrawPixel(Color.Fuchsia, Width - 2, Height);
        DrawPixel(Color.Fuchsia, Width - 3, Height);
        DrawPixel(Color.Fuchsia, Width - 4, Height);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 1);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 2);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 3);
        DrawPixel(Color.Fuchsia, Width - 2, Height - 1);
    }

    // Properties
}

internal class TUPSThemeWhite : ThemeContainer154
{
    // Fields
    private Color Accent;
    private Color Border;
    private Color TextColor;
    private Color TitleBottom;
    private Color TitleTop;
    private bool _ShowIcon;
    // Methods
    public TUPSThemeWhite()
    {
        Header = 30;
        SetColor("Titlebar Gradient Top", 0x3f, 0x3f, 0x3f);
        SetColor("Titlebar Gradient Bottom", 20, 20, 20);
        SetColor("Text", 170, 170, 170);
        SetColor("Accent", 255, 255, 255);
        SetColor("Border", Color.Black);
        TransparencyKey = Color.Fuchsia;
        BackColor = Color.FromArgb(30, 30, 30);
        Font = new Font("Segoe UI", 9f);
    }

    public bool ShowIcon
    {
        get { return _ShowIcon; }
        set
        {
            _ShowIcon = value;
            Invalidate();
        }
    }

    protected override void ColorHook()
    {
        TitleTop = GetColor("Titlebar Gradient Top");
        TitleBottom = GetColor("Titlebar Gradient Bottom");
        TextColor = GetColor("Text");
        Accent = GetColor("Accent");
        Border = GetColor("Border");
    }

    protected override void PaintHook()
    {
        base.G.Clear(Border);
        var rect = new Rectangle(1, 1, Width - 2, 0x23);
        var brush = new LinearGradientBrush(rect, TitleTop, TitleBottom, 90f);
        base.G.FillPath(brush, CreateRound(1, 1, Width - 2, 0x23, 7));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), CreateRound(1, 1, Width - 3, 0x23, 7));
        base.G.FillPath(new SolidBrush(BackColor), CreateRound(1, 0x20, Width - 2, Height - 0x21, 7));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), CreateRound(1, 0x20, Width - 3, Height - 0x22, 7));
        rect = new Rectangle(1, 0x20, Width - 2, 3);
        base.G.FillRectangle(new SolidBrush(Border), rect);
        var point = new Point(1, 0x1f);
        var point2 = new Point(Width - 2, 0x1f);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point, point2);
        var blend = new ColorBlend(3);
        blend.Colors = new[] { Color.Black, Accent, Color.Black };
        blend.Positions = new[] { 0f, 0.5f, 1f };
        rect = new Rectangle(1, 0x21, Width - 2, 2);
        DrawGradient(blend, rect, 0f);
        point2 = new Point(1, 0x23);
        point = new Point(Width - 2, 0x23);
        base.G.DrawLine(new Pen(BackColor), point2, point);
        point2 = new Point(1, 0x23);
        point = new Point(Width - 2, 0x23);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point2, point);
        if (_ShowIcon)
        {
            rect = new Rectangle(11, 8, 0x10, 0x10);
            base.G.DrawIcon(FindForm().Icon, rect);
            point2 = new Point(0x20, 8);
            base.G.DrawString(FindForm().Text, Font, new SolidBrush(TextColor), point2);
        }
        else
        {
            point2 = new Point(13, 8);
            base.G.DrawString(FindForm().Text, Font, new SolidBrush(TextColor), point2);
        }
        DrawPixel(Color.Fuchsia, 0, 0);
        DrawPixel(Color.Fuchsia, 1, 0);
        DrawPixel(Color.Fuchsia, 2, 0);
        DrawPixel(Color.Fuchsia, 3, 0);
        DrawPixel(Color.Fuchsia, 0, 1);
        DrawPixel(Color.Fuchsia, 0, 2);
        DrawPixel(Color.Fuchsia, 0, 3);
        DrawPixel(Color.Fuchsia, 1, 1);
        DrawPixel(Color.Fuchsia, Width - 1, 0);
        DrawPixel(Color.Fuchsia, Width - 2, 0);
        DrawPixel(Color.Fuchsia, Width - 3, 0);
        DrawPixel(Color.Fuchsia, Width - 4, 0);
        DrawPixel(Color.Fuchsia, Width - 1, 1);
        DrawPixel(Color.Fuchsia, Width - 1, 2);
        DrawPixel(Color.Fuchsia, Width - 1, 3);
        DrawPixel(Color.Fuchsia, Width - 2, 1);
        DrawPixel(Color.Fuchsia, 0, Height);
        DrawPixel(Color.Fuchsia, 1, Height);
        DrawPixel(Color.Fuchsia, 2, Height);
        DrawPixel(Color.Fuchsia, 3, Height);
        DrawPixel(Color.Fuchsia, 0, Height - 1);
        DrawPixel(Color.Fuchsia, 0, Height - 2);
        DrawPixel(Color.Fuchsia, 0, Height - 3);
        DrawPixel(Color.Fuchsia, 1, Height - 1);
        DrawPixel(Color.Fuchsia, Width - 1, Height);
        DrawPixel(Color.Fuchsia, Width - 2, Height);
        DrawPixel(Color.Fuchsia, Width - 3, Height);
        DrawPixel(Color.Fuchsia, Width - 4, Height);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 1);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 2);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 3);
        DrawPixel(Color.Fuchsia, Width - 2, Height - 1);
    }

    // Properties
}

internal class TUPSButton : ThemeControl154
{
    // Fields
    private Color G1;
    private Color G2;
    private Color TC;

    // Methods
    public TUPSButton()
    {
        SetColor("Gradient Top", 40, 40, 40);
        SetColor("Gradient Bottom", 20, 20, 20);
        SetColor("Text", 170, 170, 170);
    }

    protected override void ColorHook()
    {
        G1 = GetColor("Gradient Top");
        G2 = GetColor("Gradient Bottom");
        TC = GetColor("Text");
    }

    protected override void PaintHook()
    {
        Rectangle rectangle;
        base.G.Clear(BackColor);
        base.G.SmoothingMode = SmoothingMode.HighQuality;
        switch (base.State)
        {
            case MouseState.None:
                {
                    rectangle = new Rectangle(0, 0, Width - 1, Height - 1);
                    var brush = new LinearGradientBrush(rectangle, G1, G2, 90f);
                    base.G.FillPath(brush, CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.DrawPath(Pens.Black, CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White)), CreateRound(1, 1, Width - 3, Height - 3, 5));
                    break;
                }
            case MouseState.Over:
                {
                    rectangle = new Rectangle(0, 0, Width - 1, Height - 1);
                    var brush2 = new LinearGradientBrush(rectangle, G1, G2, 90f);
                    base.G.FillPath(brush2, CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.FillPath(new SolidBrush(Color.FromArgb(7, Color.White)),
                        CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.DrawPath(Pens.Black, CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White)), CreateRound(1, 1, Width - 3, Height - 3, 5));
                    break;
                }
            case MouseState.Down:
                {
                    rectangle = new Rectangle(0, 0, Width - 1, Height - 1);
                    var brush3 = new LinearGradientBrush(rectangle, G1, G2, 90f);
                    base.G.FillPath(brush3, CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.FillPath(new SolidBrush(Color.FromArgb(20, Color.Black)),
                        CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.DrawPath(Pens.Black, CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White)), CreateRound(1, 1, Width - 3, Height - 3, 5));
                    break;
                }
        }
        rectangle = new Rectangle(0, 0, Width - 1, Height);
        var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        base.G.DrawString(Text, Font, new SolidBrush(TC), rectangle, format);
    }
}

[DefaultEvent("CheckedChanged")]
internal class TUPSCheckBox : ThemeControl154
{
    // Fields
    public delegate void CheckedChangedEventHandler(object sender);

    private Color Border;
    private Color C1;
    private Color C2;
    private CheckedChangedEventHandler CheckedChangedEvent;
    private Color Glow;
    private Color TC;
    private Color UC1;
    private Color UC2;
    private int X;
    private bool _Checked;

    // Events

    // Methods
    public TUPSCheckBox()
    {
        LockHeight = 0x10;
        SetColor("Border", Color.Black);
        SetColor("Checked1", 180, 0x1a, 0x20);
        SetColor("Checked2", 200, 180, 0x1a, 0x20);
        SetColor("Unchecked1", 30, 30, 30);
        SetColor("Unchecked2", 0x19, 0x19, 0x19);
        SetColor("Glow", 15, Color.White);
        SetColor("Text", 170, 170, 170);
    }

    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }

    public event CheckedChangedEventHandler CheckedChanged;


    protected override void ColorHook()
    {
        C1 = GetColor("Checked1");
        C2 = GetColor("Checked2");
        UC1 = GetColor("Unchecked1");
        UC2 = GetColor("Unchecked2");
        Border = GetColor("Border");
        Glow = GetColor("Glow");
        TC = GetColor("Text");
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        _Checked = !_Checked;
        CheckedChangedEventHandler checkedChangedEvent = CheckedChangedEvent;
        if (checkedChangedEvent != null)
        {
            checkedChangedEvent(this);
        }
        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Invalidate();
    }

    protected override void PaintHook()
    {
        base.G.Clear(BackColor);
        if (_Checked)
        {
            DrawGradient(C1, C2, 1, 1, 14, 14);
            var point = new Point(-3, -1);
            base.G.DrawString("a", new Font("Marlett", 13f), Brushes.Black, point);
        }
        else
        {
            DrawGradient(UC1, UC2, 1, 1, 14, 14, 90f);
        }
        if ((base.State == MouseState.Over) & (X < 0x10))
        {
            if (_Checked)
            {
                base.G.FillRectangle(new SolidBrush(Glow), 1, 1, 14, 14);
            }
            else
            {
                base.G.FillRectangle(new SolidBrush(Color.FromArgb(10, Glow)), 1, 1, 14, 14);
            }
        }
        DrawBorders(new Pen(Border), 0, 0, 0x10, 0x10, 1);
        DrawText(new SolidBrush(TC), HorizontalAlignment.Left, 20, 0);
    }

    // Properties
}

internal class TUPSCheckBoxBlue : ThemeControl154
{
    // Fields
    public delegate void CheckedChangedEventHandler(object sender);

    private Color Border;
    private Color C1;
    private Color C2;
    private CheckedChangedEventHandler CheckedChangedEvent;
    private Color Glow;
    private Color TC;
    private Color UC1;
    private Color UC2;
    private int X;
    private bool _Checked;

    // Events

    // Methods
    public TUPSCheckBoxBlue()
    {
        LockHeight = 0x10;
        SetColor("Border", Color.Black);
        SetColor("Checked1", 26, 26, 180);
        SetColor("Checked2", 200, 32, 26, 180);
        SetColor("Unchecked1", 30, 30, 30);
        SetColor("Unchecked2", 25, 25, 25);
        SetColor("Glow", 15, Color.White);
        SetColor("Text", 170, 170, 170);
    }

    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }

    public event CheckedChangedEventHandler CheckedChanged;


    protected override void ColorHook()
    {
        C1 = GetColor("Checked1");
        C2 = GetColor("Checked2");
        UC1 = GetColor("Unchecked1");
        UC2 = GetColor("Unchecked2");
        Border = GetColor("Border");
        Glow = GetColor("Glow");
        TC = GetColor("Text");
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        _Checked = !_Checked;
        CheckedChangedEventHandler checkedChangedEvent = CheckedChangedEvent;
        if (checkedChangedEvent != null)
        {
            checkedChangedEvent(this);
        }
        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Invalidate();
    }

    protected override void PaintHook()
    {
        base.G.Clear(BackColor);
        if (_Checked)
        {
            DrawGradient(C1, C2, 1, 1, 14, 14);
            var point = new Point(-3, -1);
            base.G.DrawString("a", new Font("Marlett", 13f), Brushes.Black, point);
        }
        else
        {
            DrawGradient(UC1, UC2, 1, 1, 14, 14, 90f);
        }
        if ((base.State == MouseState.Over) & (X < 0x10))
        {
            if (_Checked)
            {
                base.G.FillRectangle(new SolidBrush(Glow), 1, 1, 14, 14);
            }
            else
            {
                base.G.FillRectangle(new SolidBrush(Color.FromArgb(10, Glow)), 1, 1, 14, 14);
            }
        }
        DrawBorders(new Pen(Border), 0, 0, 0x10, 0x10, 1);
        DrawText(new SolidBrush(TC), HorizontalAlignment.Left, 20, 0);
    }

    // Properties
}

internal class TUPSCheckBoxGreen : ThemeControl154
{
    // Fields
    public delegate void CheckedChangedEventHandler(object sender);

    private Color Border;
    private Color C1;
    private Color C2;
    private CheckedChangedEventHandler CheckedChangedEvent;
    private Color Glow;
    private Color TC;
    private Color UC1;
    private Color UC2;
    private int X;
    private bool _Checked;

    // Events

    // Methods
    public TUPSCheckBoxGreen()
    {
        LockHeight = 0x10;
        SetColor("Border", Color.Black);
        SetColor("Checked1", 26, 180, 26);
        SetColor("Checked2", 200, 32, 180, 32);
        SetColor("Unchecked1", 30, 30, 30);
        SetColor("Unchecked2", 25, 25, 25);
        SetColor("Glow", 15, Color.White);
        SetColor("Text", 170, 170, 170);
    }

    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }

    public event CheckedChangedEventHandler CheckedChanged;


    protected override void ColorHook()
    {
        C1 = GetColor("Checked1");
        C2 = GetColor("Checked2");
        UC1 = GetColor("Unchecked1");
        UC2 = GetColor("Unchecked2");
        Border = GetColor("Border");
        Glow = GetColor("Glow");
        TC = GetColor("Text");
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        _Checked = !_Checked;
        CheckedChangedEventHandler checkedChangedEvent = CheckedChangedEvent;
        if (checkedChangedEvent != null)
        {
            checkedChangedEvent(this);
        }
        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Invalidate();
    }

    protected override void PaintHook()
    {
        base.G.Clear(BackColor);
        if (_Checked)
        {
            DrawGradient(C1, C2, 1, 1, 14, 14);
            var point = new Point(-3, -1);
            base.G.DrawString("a", new Font("Marlett", 13f), Brushes.Black, point);
        }
        else
        {
            DrawGradient(UC1, UC2, 1, 1, 14, 14, 90f);
        }
        if ((base.State == MouseState.Over) & (X < 0x10))
        {
            if (_Checked)
            {
                base.G.FillRectangle(new SolidBrush(Glow), 1, 1, 14, 14);
            }
            else
            {
                base.G.FillRectangle(new SolidBrush(Color.FromArgb(10, Glow)), 1, 1, 14, 14);
            }
        }
        DrawBorders(new Pen(Border), 0, 0, 0x10, 0x10, 1);
        DrawText(new SolidBrush(TC), HorizontalAlignment.Left, 20, 0);
    }

    // Properties
}

internal class TUPSCheckBoxWhite : ThemeControl154
{
    // Fields
    public delegate void CheckedChangedEventHandler(object sender);

    private Color Border;
    private Color C1;
    private Color C2;
    private CheckedChangedEventHandler CheckedChangedEvent;
    private Color Glow;
    private Color TC;
    private Color UC1;
    private Color UC2;
    private int X;
    private bool _Checked;

    // Events

    // Methods
    public TUPSCheckBoxWhite()
    {
        LockHeight = 0x10;
        SetColor("Border", Color.Black);
        SetColor("Checked1", 180, 180, 180);
        SetColor("Checked2", 200, 180, 180, 180);
        SetColor("Unchecked1", 30, 30, 30);
        SetColor("Unchecked2", 25, 25, 25);
        SetColor("Glow", 15, Color.White);
        SetColor("Text", 170, 170, 170);
    }

    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }

    public event CheckedChangedEventHandler CheckedChanged;


    protected override void ColorHook()
    {
        C1 = GetColor("Checked1");
        C2 = GetColor("Checked2");
        UC1 = GetColor("Unchecked1");
        UC2 = GetColor("Unchecked2");
        Border = GetColor("Border");
        Glow = GetColor("Glow");
        TC = GetColor("Text");
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        _Checked = !_Checked;
        CheckedChangedEventHandler checkedChangedEvent = CheckedChangedEvent;
        if (checkedChangedEvent != null)
        {
            checkedChangedEvent(this);
        }
        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Invalidate();
    }

    protected override void PaintHook()
    {
        base.G.Clear(BackColor);
        if (_Checked)
        {
            DrawGradient(C1, C2, 1, 1, 14, 14);
            var point = new Point(-3, -1);
            base.G.DrawString("a", new Font("Marlett", 13f), Brushes.Black, point);
        }
        else
        {
            DrawGradient(UC1, UC2, 1, 1, 14, 14, 90f);
        }
        if ((base.State == MouseState.Over) & (X < 0x10))
        {
            if (_Checked)
            {
                base.G.FillRectangle(new SolidBrush(Glow), 1, 1, 14, 14);
            }
            else
            {
                base.G.FillRectangle(new SolidBrush(Color.FromArgb(10, Glow)), 1, 1, 14, 14);
            }
        }
        DrawBorders(new Pen(Border), 0, 0, 0x10, 0x10, 1);
        DrawText(new SolidBrush(TC), HorizontalAlignment.Left, 20, 0);
    }

    // Properties
}

internal class TUPSComboBox : ComboBox
{
    // Fields
    private GraphicsPath CreateRoundPath;
    private Rectangle CreateRoundRectangle;
    private int X;

    // Methods
    public TUPSComboBox()
    {
        base.DropDownClosed += GhostComboBox_DropDownClosed;
        base.TextChanged += GhostCombo_TextChanged;
        SetStyle(
            ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint, true);
        ForeColor = Color.FromArgb(170, 170, 170);
        BackColor = Color.FromArgb(30, 30, 30);
        DrawMode = DrawMode.OwnerDrawFixed;
        ItemHeight = 0x11;
        DropDownStyle = ComboBoxStyle.DropDownList;
    }


    public GraphicsPath CreateRound(Rectangle r, int slope)
    {
        CreateRoundPath = new GraphicsPath(FillMode.Winding);
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
        CreateRoundPath.CloseFigure();
        return CreateRoundPath;
    }

    public GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
    {
        CreateRoundRectangle = new Rectangle(x, y, width, height);
        return CreateRound(CreateRoundRectangle, slope);
    }

    private void GhostCombo_TextChanged(object sender, EventArgs e)
    {
        Invalidate();
    }

    private void GhostComboBox_DropDownClosed(object sender, EventArgs e)
    {
        DropDownStyle = ComboBoxStyle.Simple;
        Application.DoEvents();
        DropDownStyle = ComboBoxStyle.DropDownList;
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (e.Index >= 0)
        {
            var rectangle = new Rectangle
            {
                X = e.Bounds.X,
                Y = e.Bounds.Y,
                Width = e.Bounds.Width - 1,
                Height = e.Bounds.Height - 1
            };
            e.DrawBackground();
            if ((e.State ==
                 (DrawItemState.NoFocusRect | DrawItemState.NoAccelerator | DrawItemState.Focus | DrawItemState.Selected)) |
                (e.State == (DrawItemState.Focus | DrawItemState.Selected)))
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(70, 70, 70)), e.Bounds);
                e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, Brushes.White, e.Bounds.X, e.Bounds.Y);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColor), e.Bounds);
                e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, Brushes.White, e.Bounds.X, e.Bounds.Y);
            }
            base.OnDrawItem(e);
        }
    }

    protected override void OnDropDownClosed(EventArgs e)
    {
        base.OnDropDownClosed(e);
        X = -1;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        X = -1;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (DropDownStyle != ComboBoxStyle.DropDownList)
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }
        var image = new Bitmap(Width, Height);
        Graphics graphics = Graphics.FromImage(image);
        graphics.Clear(BackColor);
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        var brush = new LinearGradientBrush(rect, Color.FromArgb(40, 40, 40), Color.FromArgb(20, 20, 20), 90f);
        graphics.FillPath(brush, CreateRound(0, 0, Width - 1, Height - 1, 5));
        if (X > (Width - 0x1a))
        {
            rect = new Rectangle(Width - 0x19, 2, 0x18, Height - 4);
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(5, Color.White)), rect);
        }
        graphics.DrawPath(Pens.Black, CreateRound(0, 0, Width - 1, Height - 1, 5));
        graphics.DrawPath(new Pen(Color.FromArgb(15, Color.White)), CreateRound(1, 1, Width - 3, Height - 3, 5));
        var point = new Point(Width - 0x19, 0);
        var point2 = new Point(Width - 0x19, Height);
        graphics.DrawLine(Pens.Black, point, point2);
        point2 = new Point(Width - 0x18, 2);
        point = new Point(Width - 0x18, Height - 3);
        graphics.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point2, point);
        point2 = new Point(Width - 0x1a, 2);
        point = new Point(Width - 0x1a, Height - 3);
        graphics.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point2, point);
        var num = (int)Math.Round(graphics.MeasureString(" ... ", Font).Height);
        if (SelectedIndex != -1)
        {
            graphics.DrawString((Items[SelectedIndex]).ToString(), Font, new SolidBrush(ForeColor), 4f,
                (Height / 2) - (num / 2));
        }
        else if ((Items != null) & (Items.Count > 0))
        {
            graphics.DrawString((Items[0]).ToString(), Font, new SolidBrush(ForeColor), 4f, (Height / 2) - (num / 2));
        }
        else
        {
            graphics.DrawString(" ... ", Font, new SolidBrush(ForeColor), 4f, (Height / 2) - (num / 2));
        }
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        var pointArray = new Point[3];
        point2 = new Point(Width - 0x12, 9);
        pointArray[0] = point2;
        point = new Point(Width - 10, 9);
        pointArray[1] = point;
        var point3 = new Point(Width - 14, 14);
        pointArray[2] = point3;
        Point[] points = pointArray;
        graphics.FillPolygon(new SolidBrush(Color.FromArgb(170, 170, 170)), points);
        e.Graphics.DrawImage((Image)image.Clone(), 0, 0);

        graphics.Dispose();
        image.Dispose();
    }

    public Point[] Triangle(Point Location, Size Size)
    {
        return new[]
        {
            Location, new Point(Location.X + Size.Width, Location.Y),
            new Point(Location.X + (Size.Width/2), Location.Y + Size.Height), Location
        };
    }
}

internal class TUPSControlBox_TwoButtons : ThemeControl154
{
    // Fields
    private Color G1;
    private Color G2;
    private Color G3;
    private Color I;
    private Color O;
    private int X;

    // Methods
    public TUPSControlBox_TwoButtons()
    {
        SetColor("Gradient Top", 0x3e, 0x3e, 0x3e);
        SetColor("Gradient Middle", 0x2c, 0x2c, 0x2c);
        SetColor("Gradient Bottom", 0x1b, 0x1b, 0x1b);
        SetColor("Icons", 170, 170, 170);
        SetColor("Outline", 90, Color.Black);
        var size = new Size(0x35, 0x1c);
        Size = size;
        Anchor = AnchorStyles.Right | AnchorStyles.Top;
    }

    protected override void ColorHook()
    {
        G1 = GetColor("Gradient Top");
        G2 = GetColor("Gradient Middle");
        G3 = GetColor("Gradient Bottom");
        I = GetColor("Icons");
        O = GetColor("Outline");
    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        if (X < 30)
        {
            FindForm().WindowState = FormWindowState.Minimized;
        }
        else if (X > 30)
        {
            FindForm().Close();
        }
    }

    protected override void OnLocationChanged(EventArgs e)
    {
        base.OnLocationChanged(e);
        Top = 2;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Invalidate();
    }

    protected override void PaintHook()
    {
        base.G.Clear(BackColor);
        var rect = new Rectangle(0, 0, Width, Height);
        var brush = new LinearGradientBrush(rect, G1, G2, 90f);
        var blend = new ColorBlend(3);
        blend.Colors = new[] { G1, G2, G3 };
        blend.Positions = new[] { 0f, 0.5f, 1f };
        brush.InterpolationColors = blend;
        rect = new Rectangle(0, 0, Width, Height);
        base.G.FillRectangle(brush, rect);
        base.G.SmoothingMode = SmoothingMode.HighQuality;
        if (base.State == MouseState.Over)
        {
            if (X < 30)
            {
                base.G.FillPath(new SolidBrush(Color.FromArgb(20, Color.Black)), CreateRound(4, 4, 0x16, 0x12, 6));
                base.G.DrawPath(new Pen(O), CreateRound(4, 4, 0x16, 0x12, 6));
            }
            else if (X > 30)
            {
                base.G.FillPath(new SolidBrush(Color.FromArgb(20, Color.Black)), CreateRound(0x1b, 4, 0x17, 0x12, 6));
                base.G.DrawPath(new Pen(O), CreateRound(0x1b, 4, 0x17, 0x12, 6));
            }
        }
        else if (base.State == MouseState.Down)
        {
            if (X < 30)
            {
                base.G.FillPath(new SolidBrush(Color.FromArgb(70, Color.Black)), CreateRound(4, 4, 0x16, 0x12, 6));
                base.G.DrawPath(new Pen(O), CreateRound(4, 4, 0x16, 0x12, 6));
            }
            else if (X > 30)
            {
                base.G.FillPath(new SolidBrush(Color.FromArgb(70, Color.Black)), CreateRound(0x1b, 4, 0x17, 0x12, 6));
                base.G.DrawPath(new Pen(O), CreateRound(0x1b, 4, 0x17, 0x12, 6));
            }
        }
        var point = new Point(8, 7);
        base.G.DrawString("0", new Font("Marlett", 10f), new SolidBrush(I), point);
        point = new Point(0x1f, 7);
        base.G.DrawString("r", new Font("Marlett", 10f), new SolidBrush(I), point);
    }
}

internal class TUPSGroupBox : ThemeContainer154
{
    // Fields
    private Color B;
    private Color G1;
    private Color G2;
    private Color TC;

    // Methods
    public TUPSGroupBox()
    {
        ControlMode = true;
        SetColor("Gradient Top", 40, 40, 40);
        SetColor("Gradient Bottom", 20, 20, 20);
        SetColor("Text", 170, 170, 170);
        SetColor("Border", Color.Black);
    }

    protected override void ColorHook()
    {
        G1 = GetColor("Gradient Top");
        G2 = GetColor("Gradient Bottom");
        TC = GetColor("Text");
        B = GetColor("Border");
    }

    protected override void PaintHook()
    {
        base.G.Clear(BackColor);
        base.G.SmoothingMode = SmoothingMode.HighQuality;
        base.G.DrawPath(new Pen(B), CreateRound(0, 0, Width - 1, Height - 1, 7));
        var rect = new Rectangle(0, 0, Width - 1, 0x1b);
        var brush = new LinearGradientBrush(rect, G1, G2, 90f);
        base.G.FillPath(brush, CreateRound(0, 0, Width - 1, 0x1b, 7));
        base.G.DrawPath(new Pen(B), CreateRound(0, 0, Width - 1, 0x1b, 7));
        base.G.SmoothingMode = SmoothingMode.None;
        rect = new Rectangle(1, 0x18, Width - 2, 10);
        base.G.FillRectangle(new SolidBrush(BackColor), rect);
        var point = new Point(0, 0x18);
        var point2 = new Point(Width, 0x18);
        base.G.DrawLine(new Pen(B), point, point2);
        point2 = new Point(2, 0x17);
        point = new Point(Width - 3, 0x17);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point2, point);
        point2 = new Point(7, 5);
        base.G.DrawString(Text, Font, new SolidBrush(TC), point2);
        base.G.SmoothingMode = SmoothingMode.HighQuality;
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White)), CreateRound(1, 1, Width - 3, Height - 3, 7));
    }
}

internal class TUPSListbox : ListBox
{
    // Fields

    // Methods
    public TUPSListbox()
    {
        SetStyle(ControlStyles.DoubleBuffer, true);
        BorderStyle = BorderStyle.None;
        DrawMode = DrawMode.OwnerDrawFixed;
        ItemHeight = 20;
        ForeColor = Color.FromArgb(170, 170, 170);
        BackColor = Color.FromArgb(0x16, 0x16, 0x16);
        IntegralHeight = false;
    }


    public void CustomPaint()
    {
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        CreateGraphics().DrawRectangle(Pens.Black, rect);
        rect = new Rectangle(1, 1, Width - 3, Height - 3);
        CreateGraphics().DrawRectangle(new Pen(Color.FromArgb(0x2b, 0x2b, 0x2b)), rect);
        rect = new Rectangle(0, 0, 1, 1);
        CreateGraphics().FillRectangle(new SolidBrush(BackColor), rect);
        rect = new Rectangle(Width - 1, Height - 1, 1, 1);
        CreateGraphics().FillRectangle(new SolidBrush(BackColor), rect);
        rect = new Rectangle(0, Height - 1, 1, 1);
        CreateGraphics().FillRectangle(new SolidBrush(BackColor), rect);
        rect = new Rectangle(Width - 1, 0, 1, 1);
        CreateGraphics().FillRectangle(new SolidBrush(BackColor), rect);
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        try
        {
            if (e.Index >= 0)
            {
                Rectangle bounds;
                e.DrawBackground();
                var location = new Point(e.Bounds.Left, e.Bounds.Top + 2);
                var size = new Size(Bounds.Width, 0x10);
                var rectangle = new Rectangle(location, size);
                e.DrawFocusRectangle();
                if (e.State.HasFlag(DrawItemState.Selected))
                {
                    var point2 = new Point(e.Bounds.Location.X + 2, e.Bounds.Location.Y);
                    size = new Size(e.Bounds.Width - 4, e.Bounds.Height);
                    var rect = new Rectangle(point2, size);
                    var brush = new LinearGradientBrush(rect, Color.FromArgb(170, 15, 0x16),
                        Color.FromArgb(130, 15, 0x16), 90f);
                    e.Graphics.FillRectangle(new SolidBrush(BackColor), e.Bounds);
                    e.Graphics.FillRectangle(brush, rect);
                    brush.Dispose();
                    bounds = e.Bounds;
                    e.Graphics.DrawString(" " + Items[e.Index], Font, Brushes.White, e.Bounds.X, bounds.Y + 1);
                }
                else
                {
                    bounds = e.Bounds;
                    e.Graphics.DrawString(" " + Items[e.Index], Font, new SolidBrush(ForeColor), e.Bounds.X,
                        bounds.Y + 1);
                }
                bounds = new Rectangle(0, 0, Width - 1, Height - 1);
                e.Graphics.DrawRectangle(Pens.Black, bounds);
                bounds = new Rectangle(1, 1, Width - 3, Height - 3);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(0x2b, 0x2b, 0x2b)), bounds);
                bounds = new Rectangle(0, 0, 1, 1);
                e.Graphics.FillRectangle(new SolidBrush(BackColor), bounds);
                bounds = new Rectangle(Width - 1, Height - 1, 1, 1);
                e.Graphics.FillRectangle(new SolidBrush(BackColor), bounds);
                bounds = new Rectangle(0, Height - 1, 1, 1);
                e.Graphics.FillRectangle(new SolidBrush(BackColor), bounds);
                bounds = new Rectangle(Width - 1, 0, 1, 1);
                e.Graphics.FillRectangle(new SolidBrush(BackColor), bounds);
                base.OnDrawItem(e);
            }
        }
        catch
        {
        }
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (m.Msg == 15)
        {
            CustomPaint();
        }
    }
}

internal class TUPSProgressBar : ThemeControl154
{
    // Fields
    private Color Edge;
    private Color G1;
    private double ROffset;
    private int _Maximum = 100;
    private int _Minimum;
    private int _Value;

    // Methods
    public TUPSProgressBar()
    {
        SetColor("Color", 180, 0x1a, 0x20);
        SetColor("Edge", Color.Black);
    }

    // Properties
    public bool Animated
    {
        get { return IsAnimated; }
        set
        {
            IsAnimated = value;
            Invalidate();
        }
    }

    public int Maximum
    {
        get { return _Maximum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }
            _Maximum = value;
            if (value < _Value)
            {
                _Value = value;
            }
            if (value < _Minimum)
            {
                _Minimum = value;
            }
            Invalidate();
        }
    }

    public int Minimum
    {
        get { return _Minimum; }
        set
        {
            if (value < 0)
            {
                throw new Exception("Property value is not valid.");
            }
            _Minimum = value;
            if (value > _Value)
            {
                _Value = value;
            }
            if (value > _Maximum)
            {
                _Maximum = value;
            }
            Invalidate();
        }
    }

    public int Value
    {
        get { return _Value; }
        set
        {
            if ((value > _Maximum) || (value < _Minimum))
            {
                throw new Exception("Property value is not valid.");
            }
            _Value = value;
            Invalidate();
        }
    }

    protected override void ColorHook()
    {
        G1 = GetColor("Color");
        Edge = GetColor("Edge");
    }

    private void Increment(int amount)
    {
        Value += amount;
    }

    protected override void OnAnimation()
    {
        base.OnAnimation();
        if (ROffset < 7.0)
        {
            ROffset += 0.2;
        }
        else
        {
            ROffset = 0.0;
        }
        Invalidate();
    }

    protected override void PaintHook()
    {
        Point point;
        base.G.Clear(BackColor);
        var rect = new Rectangle(1, 1, (int)Math.Round(((Width / ((double)Maximum)) * Value) - 1.0), Height - 2);
        var brush = new LinearGradientBrush(rect, Color.FromArgb(180, G1), G1, 0f);
        var brush2 = new HatchBrush(HatchStyle.ForwardDiagonal, Color.FromArgb(20, Color.Black), Color.Transparent);
        if (Value > 1)
        {
            base.G.FillPath(Brushes.Black,
                CreateRound(1, 1, (int)Math.Round(((Width / ((double)Maximum)) * Value) - 1.0), Height - 2, 5));
            base.G.FillPath(brush,
                CreateRound(1, 1, (int)Math.Round(((Width / ((double)Maximum)) * Value) - 1.0), Height - 2, 5));
            point = new Point((int)Math.Round(-ROffset), 0);
            base.G.RenderingOrigin = point;
            base.G.FillPath(brush2,
                CreateRound(1, 1, (int)Math.Round(((Width / ((double)Maximum)) * Value) - 1.0), Height - 2, 5));
            point = new Point((int)Math.Round(-ROffset + 1.0), 0);
            base.G.RenderingOrigin = point;
            base.G.FillPath(brush2,
                CreateRound(1, 1, (int)Math.Round(((Width / ((double)Maximum)) * Value) - 1.0), Height - 2, 5));
            point = new Point((int)Math.Round(-ROffset + 2.0), 0);
            base.G.RenderingOrigin = point;
            base.G.FillPath(brush2,
                CreateRound(1, 1, (int)Math.Round(((Width / ((double)Maximum)) * Value) - 1.0), Height - 2, 5));
            base.G.FillPath(new SolidBrush(Color.FromArgb(0x23, Color.Black)),
                CreateRound(1, (int)Math.Round(Height / 2.0), (int)Math.Round(((Width / ((double)Maximum)) * Value) - 1.0),
                    (int)Math.Round(Height / 2.0), 5));
        }
        point = new Point((int)Math.Round(((Width / ((double)Maximum)) * Value) - 1.0), 2);
        var point2 = new Point((int)Math.Round(((Width / ((double)Maximum)) * Value) - 1.0), Height - 2);
        base.G.DrawLine(new Pen(Edge), point, point2);
        base.G.SmoothingMode = SmoothingMode.HighQuality;
        base.G.DrawPath(new Pen(Edge), CreateRound(1, 1, Width - 2, Height - 2, 5));
        base.G.DrawPath(new Pen(Color.FromArgb(10, Color.White)), CreateRound(2, 2, Width - 4, Height - 4, 5));
    }
}

[DefaultEvent("CheckedChanged")]
internal class TUPSRadiobutton : ThemeControl154
{
    // Fields
    public delegate void CheckedChangedEventHandler(object sender);

    private Color Border;
    private Color C1;
    private Color C2;
    private CheckedChangedEventHandler CheckedChangedEvent;
    private Color Glow;
    private Color TC;
    private Color UC1;
    private Color UC2;
    private int X;
    private bool _Checked;

    // Events

    // Methods
    public TUPSRadiobutton()
    {
        LockHeight = 0x10;
        SetColor("Border", Color.Black);
        SetColor("Checked1", 180, 0x1a, 0x20);
        SetColor("Checked2", 200, 180, 0x1a, 0x20);
        SetColor("Unchecked1", 30, 30, 30);
        SetColor("Unchecked2", 0x19, 0x19, 0x19);
        SetColor("Glow", 15, Color.White);
        SetColor("Text", 170, 170, 170);
    }

    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            InvalidateControls();
            CheckedChangedEventHandler checkedChangedEvent = CheckedChangedEvent;
            if (checkedChangedEvent != null)
            {
                checkedChangedEvent(this);
            }
            Invalidate();
        }
    }

    public event CheckedChangedEventHandler CheckedChanged;


    protected override void ColorHook()
    {
        C1 = GetColor("Checked1");
        C2 = GetColor("Checked2");
        UC1 = GetColor("Unchecked1");
        UC2 = GetColor("Unchecked2");
        Border = GetColor("Border");
        Glow = GetColor("Glow");
        TC = GetColor("Text");
    }

    private void InvalidateControls()
    {
        if (IsHandleCreated && _Checked)
        {
            IEnumerator enumerator = null;
            try
            {
                enumerator = Parent.Controls.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var current = (Control)enumerator.Current;
                    if ((((current == this) || !(current is TUPSRadiobutton)) ? 0 : 1) != 0)
                    {
                        ((TUPSRadiobutton)current).Checked = false;
                    }
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    (enumerator as IDisposable).Dispose();
                }
            }
        }
    }

    protected override void OnCreation()
    {
        InvalidateControls();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (!_Checked)
        {
            Checked = true;
        }
        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Invalidate();
    }

    protected override void PaintHook()
    {
        Rectangle rectangle;
        Rectangle rectangle2;
        base.G.Clear(BackColor);
        base.G.SmoothingMode = SmoothingMode.HighQuality;
        if (_Checked)
        {
            rectangle = new Rectangle(0, 0, 15, 15);
            rectangle2 = new Rectangle(0, 0, 15, 15);
            base.G.FillEllipse(new LinearGradientBrush(rectangle, C1, C2, 90f), rectangle2);
        }
        else
        {
            rectangle2 = new Rectangle(0, 0, 15, 15);
            rectangle = new Rectangle(0, 0, 15, 15);
            base.G.FillEllipse(new LinearGradientBrush(rectangle2, UC1, UC2, 90f), rectangle);
        }
        if ((base.State == MouseState.Over) & (X < 0x10))
        {
            if (Checked)
            {
                base.G.FillEllipse(new SolidBrush(Glow), 0, 0, 15, 15);
            }
            else
            {
                base.G.FillEllipse(new SolidBrush(Color.FromArgb(10, Glow)), 0, 0, 15, 15);
            }
        }
        rectangle2 = new Rectangle(0, 0, 15, 15);
        base.G.DrawEllipse(new Pen(Border), rectangle2);
        DrawText(new SolidBrush(TC), HorizontalAlignment.Left, 20, 0);
    }

    // Properties
}

internal class TUPSSeparator : ThemeControl154
{
    // Fields
    private Color Accent;
    private Color Border;

    // Methods
    public TUPSSeparator()
    {
        SetColor("Border", Color.Black);
        SetColor("Accent", 180, 0x1a, 0x20);
        LockHeight = 5;
    }

    protected override void ColorHook()
    {
        Border = GetColor("Border");
        Accent = GetColor("Accent");
    }

    protected override void PaintHook()
    {
        base.G.Clear(BackColor);
        var point = new Point(0, 0);
        var point2 = new Point(Width, 0);
        base.G.DrawLine(new Pen(Color.FromArgb(10, Color.White)), point, point2);
        point2 = new Point(0, 1);
        point = new Point(Width, 1);
        base.G.DrawLine(new Pen(Border), point2, point);
        var blend = new ColorBlend(3);
        blend.Colors = new[] { Color.Black, Accent, Color.Black };
        blend.Positions = new[] { 0f, 0.5f, 1f };
        var r = new Rectangle(1, 2, Width - 2, 2);
        DrawGradient(blend, r, 0f);
        point2 = new Point(0, 4);
        point = new Point(Width, 4);
        base.G.DrawLine(new Pen(Color.FromArgb(10, Color.White)), point2, point);
    }
}

internal class TUPSTabcontrol : TabControl
{
    // Fields
    private readonly Pen Border;

    // Methods
    public TUPSTabcontrol()
    {
        Border = Pens.Black;
        SetStyle(
            ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint, true);
        DoubleBuffered = true;
        SizeMode = TabSizeMode.Fixed;
        var size = new Size(0x2c, 0x88);
        ItemSize = size;
    }


    protected override void CreateHandle()
    {
        base.CreateHandle();
        Alignment = TabAlignment.Left;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Rectangle tabRect;
        Point point4;
        Point point5;
        Size size3;
        StringFormat format;
        var image = new Bitmap(Width, Height);
        Graphics graphics = Graphics.FromImage(image);
        try
        {
            SelectedTab.BackColor = Color.FromArgb(30, 30, 30);
        }
        catch
        {
        }
        graphics.Clear(Color.FromArgb(30, 30, 30));
        var point = new Point(ItemSize.Height + 3, 0);
        var location = new Point(ItemSize.Height + 3, 0x3e7);
        graphics.DrawLine(Border, point, location);
        Size itemSize = ItemSize;
        location = new Point(itemSize.Height + 2, 0);
        point = new Point(ItemSize.Height + 2, 0x3e7);
        graphics.DrawLine(new Pen(Color.FromArgb(15, Color.White)), location, point);
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        graphics.DrawRectangle(Border, rect);
        rect = new Rectangle(1, 1, Width - 3, Height - 3);
        graphics.DrawRectangle(new Pen(Color.FromArgb(15, Color.White)), rect);
        int num = TabCount - 1;
        int index = 0;
    Label_0147:
        if (index > num)
        {
            e.Graphics.DrawImage((Image)image.Clone(), 0, 0);
            graphics.Dispose();
            image.Dispose();
            return;
        }
        if (index == SelectedIndex)
        {
            Rectangle rectangle2;
            Point point3;
            if (index == -1)
            {
                point = GetTabRect(index).Location;
                point3 = new Point(GetTabRect(index).Location.X - 2, point.Y - 2);
                itemSize = new Size(GetTabRect(index).Width + 3, GetTabRect(index).Height + 1);
                rectangle2 = new Rectangle(point3, itemSize);
            }
            else
            {
                tabRect = GetTabRect(index);
                point = new Point(tabRect.Location.X - 2, GetTabRect(index).Location.Y - 2);
                itemSize = new Size(GetTabRect(index).Width + 3, GetTabRect(index).Height);
                rectangle2 = new Rectangle(point, itemSize);
            }
            var blend = new ColorBlend();
            blend.Colors = new[] { Color.FromArgb(40, 40, 40), Color.FromArgb(30, 30, 30), Color.FromArgb(20, 20, 20) };
            blend.Positions = new[] { 0f, 0.5f, 1f };
            var brush = new LinearGradientBrush(rectangle2, Color.Black, Color.Black, 90f)
            {
                InterpolationColors = blend
            };
            graphics.FillRectangle(brush, rectangle2);
            graphics.DrawRectangle(Border, rectangle2);
            tabRect = new Rectangle(rectangle2.Location.X + 1, rectangle2.Location.Y + 1, rectangle2.Width - 2,
                rectangle2.Height - 2);
            graphics.DrawRectangle(new Pen(Color.FromArgb(15, Color.White)), tabRect);
            location = GetTabRect(index).Location;
            point = new Point(GetTabRect(index).Location.X - 2, location.Y - 2);
            itemSize = new Size(GetTabRect(index).Width + 3, GetTabRect(index).Height + 1);
            rectangle2 = new Rectangle(point, itemSize);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            var pointArray = new Point[3];
            itemSize = ItemSize;
            tabRect = GetTabRect(index);
            point3 = tabRect.Location;
            location = new Point(itemSize.Height - 3, point3.Y + 20);
            pointArray[0] = location;
            point = GetTabRect(index).Location;
            point4 = new Point(ItemSize.Height + 4, point.Y + 14);
            pointArray[1] = point4;
            size3 = ItemSize;
            point5 = new Point(size3.Height + 4, GetTabRect(index).Location.Y + 0x1b);
            pointArray[2] = point5;
            Point[] points = pointArray;
            graphics.DrawPolygon(new Pen(Color.FromArgb(15, Color.White), 3f), points);
            graphics.FillPolygon(new SolidBrush(Color.FromArgb(30, 30, 30)), points);
            graphics.DrawPolygon(Border, points);
            if (ImageList != null)
            {
                try
                {
                    if (ImageList.Images[TabPages[index].ImageIndex] != null)
                    {
                        point5 = rectangle2.Location;
                        point4 = new Point(point5.X + 8, rectangle2.Location.Y + 6);
                        graphics.DrawImage(ImageList.Images[TabPages[index].ImageIndex], point4);
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString("      " + TabPages[index].Text, Font, Brushes.DimGray, rectangle2, format);
                    }
                    else
                    {
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString(TabPages[index].Text, Font, Brushes.White, rectangle2, format);
                    }
                    goto Label_09D5;
                }
                catch
                {
                    format = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };
                    graphics.DrawString(TabPages[index].Text, Font, Brushes.White, rectangle2, format);
                    goto Label_09D5;
                }
            }
            format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            graphics.DrawString(TabPages[index].Text, Font, Brushes.White, rectangle2, format);
        }
        else
        {
            tabRect = GetTabRect(index);
            point5 = tabRect.Location;
            Point point6 = GetTabRect(index).Location;
            point4 = new Point(point5.X - 1, point6.Y - 1);
            size3 = new Size(GetTabRect(index).Width + 2, GetTabRect(index).Height);
            var layoutRectangle = new Rectangle(point4, size3);
            point5 = new Point(layoutRectangle.Right, layoutRectangle.Top);
            point6 = new Point(layoutRectangle.Right, layoutRectangle.Bottom);
            graphics.DrawLine(Border, point5, point6);
            point5 = new Point(layoutRectangle.Right - 1, layoutRectangle.Top);
            point6 = new Point(layoutRectangle.Right - 1, layoutRectangle.Bottom);
            graphics.DrawLine(new Pen(Color.FromArgb(0x2b, 0x2b, 0x2b)), point5, point6);
            if (ImageList != null)
            {
                try
                {
                    if (ImageList.Images[TabPages[index].ImageIndex] != null)
                    {
                        point5 = layoutRectangle.Location;
                        point4 = new Point(point5.X + 8, layoutRectangle.Location.Y + 6);
                        graphics.DrawImage(ImageList.Images[TabPages[index].ImageIndex], point4);
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString("      " + TabPages[index].Text, Font,
                            new SolidBrush(Color.FromArgb(170, 170, 170)), layoutRectangle, format);
                    }
                    else
                    {
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString(TabPages[index].Text, Font, new SolidBrush(Color.FromArgb(170, 170, 170)),
                            layoutRectangle, format);
                    }
                    goto Label_09D5;
                }
                catch
                {
                    format = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };
                    graphics.DrawString(TabPages[index].Text, Font, new SolidBrush(Color.FromArgb(170, 170, 170)),
                        layoutRectangle, format);
                    goto Label_09D5;
                }
            }
            format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            graphics.DrawString(TabPages[index].Text, Font, new SolidBrush(Color.FromArgb(170, 170, 170)),
                layoutRectangle, format);
        }
    Label_09D5:
        index++;
        goto Label_0147;
    }

    public Brush ToBrush(Color color)
    {
        return new SolidBrush(color);
    }

    public Pen ToPen(Color color)
    {
        return new Pen(color);
    }
}

public sealed class AnimatedVerticalTabControl : TabControl
{
    private int _oldIndex = 1;
    private bool _transitionEnabled = true;
    private int _transitionSpeed = 20;

    /// <summary>
    ///     Creates a new instance of the <see cref="AnimatedVerticalTabControl" />
    /// </summary>
    public AnimatedVerticalTabControl()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint |
            ControlStyles.DoubleBuffer, true);
        SizeMode = TabSizeMode.Fixed;
        ItemSize = new Size(33, 136);
        Alignment = TabAlignment.Left;
    }

    /// <summary>
    ///     The speed of the transition.
    /// </summary>
    public int TransitionSpeed
    {
        get { return _transitionSpeed; }
        set { _transitionSpeed = value; }
    }

    /// <summary>
    ///     Enables or disables transitions when switching between <see cref="TabPage" />s.
    /// </summary>
    public bool TransitionEnabled
    {
        get { return _transitionEnabled; }
        set { _transitionEnabled = value; }
    }

    /// <summary>
    ///     Raises the <see cref="Control.CreateControl" /> method.
    /// </summary>
    protected override void OnCreateControl()
    {
        if (!DesignMode)
        {
            TransitionEnabled = false;
            for (int i = 0; i < TabPages.Count; i++)
            {
                SelectedIndex = i;
            }
            TransitionEnabled = true;
            SelectedIndex = 0;
        }
    }

    /// <summary>
    ///     Raises the <see cref="Control.Paint" /> event.
    /// </summary>
    /// <param name="e">
    ///     A <see cref="System.Windows.Forms.PaintEventArgs" /> that contains the event
    ///     data.
    /// </param>
    protected override void OnPaint(PaintEventArgs e)
    {
        var bitmap = new Bitmap(Width, Height);


        Graphics graphics = Graphics.FromImage(bitmap);
        //COLOR: Hintergrund PageViewField

        try
        {
            if (SelectedTab != null)
                SelectedTab.BackColor = Color.FromArgb(30, 30, 30);
        }
        catch (Exception)
        {
        }

        graphics.Clear(Color.FromArgb(31, 31, 31));

        //COLOR: Hintergrund TabView (Tab-Behälter hiintergrund)
        var T = new HatchBrush(HatchStyle.Trellis, Color.FromArgb(30, 30, 30), Color.FromArgb(30, 30, 30));
        graphics.FillRectangle(T, new Rectangle(Point.Empty, new Size(ItemSize.Height + 4, Height)));

        graphics.DrawLine(new Pen(Color.FromArgb(44, 44, 44)), new Point(ItemSize.Height + 2, 0),
            new Point(ItemSize.Height + 2, 999));
        graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), new Point(ItemSize.Height + 3, 0),
            new Point(ItemSize.Height + 3, 999));

        for (int i = 0; i <= TabCount - 1; i++)
        {
            var tabRect =
                new Rectangle(new Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2),
                    new Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1));
            if (i == SelectedIndex)
            {
                var colorBlend = new ColorBlend
                {
                    Colors = new[]
                    {
                        Color.FromArgb(40, 40, 40),
                        Color.FromArgb(30, 30, 30),
                        Color.FromArgb(20, 20, 20)
                        //G.Clear(Color.FromArgb(24, 24, 24));
                        //Pen P = new Pen(Color.FromArgb(13, Color.FromArgb(24,24,24)));
 
                        //G.FillRectangle(T, 0, 0, Width, Height);
                        //Rectangle x2 = new Rectangle(new Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), new Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1));
                        //ColorBlend myBlend = new ColorBlend();
                        //myBlend.Colors = new[] { Color.FromArgb(214, 166, 0), Color.FromArgb(33, 33, 33), Color.FromArgb(96, 110, 121) };
                    },
                    Positions = new[] { 0f, 0.5f, 1f }
                };


                //ItemItself
                var A = new HatchBrush(HatchStyle.Trellis, Color.FromArgb(24, 24, 24), Color.FromArgb(8, 8, 8));
                var OrangeCarbon = new HatchBrush(HatchStyle.Trellis, Color.FromArgb(24, 24, 24),
                    Color.FromArgb(20, 20, 20));
                var gradientBrush = new LinearGradientBrush(tabRect, Color.FromArgb(30, 30, 30),
                    Color.FromArgb(30, 30, 30), 90F)
                {
                    InterpolationColors = colorBlend
                };


                graphics.FillRectangle(gradientBrush, tabRect);
                graphics.DrawRectangle(new Pen(Color.FromArgb(30, 30, 30)), tabRect);
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                Point[] fillPoints =
                {
                    new Point(ItemSize.Height - 3, GetTabRect(i).Location.Y + 20),
                    new Point(ItemSize.Height + 4, GetTabRect(i).Location.Y + 14),
                    new Point(ItemSize.Height + 4, GetTabRect(i).Location.Y + 27)
                };

                graphics.FillPolygon(T, fillPoints);
                graphics.DrawPolygon(new Pen(Color.FromArgb(20, 20, 20)), fillPoints);

                graphics.DrawString(TabPages[i].Text, new Font(Font.FontFamily, Font.Size, FontStyle.Bold),
                    Brushes.White, tabRect, new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    });

                graphics.DrawLine(new Pen(Color.FromArgb(20, 20, 20)),
                    new Point(tabRect.Location.X - 1, tabRect.Location.Y - 1),
                    new Point(tabRect.Location.X, tabRect.Location.Y));

                graphics.DrawLine(new Pen(Color.FromArgb(20, 20, 20)),
                    new Point(tabRect.Location.X - 1, tabRect.Bottom - 1),
                    new Point(tabRect.Location.X, tabRect.Bottom));
            }
            else
            {
                var gradientBrush = new LinearGradientBrush(tabRect, Color.FromArgb(30, 30, 30),
                    Color.FromArgb(30, 30, 30), 90F);


                var gradientBrushOrangeCarbon = new HatchBrush(HatchStyle.Cross, Color.FromArgb(30, 30, 30),
                    Color.FromArgb(30, 30, 30));
                graphics.FillRectangle(gradientBrush, tabRect);
                graphics.DrawLine(new Pen(Color.FromArgb(30, 30, 30)), new Point(tabRect.Right, tabRect.Top),
                    new Point(tabRect.Right, tabRect.Bottom));

                graphics.DrawString(TabPages[i].Text, Font,
                    Brushes.White, tabRect, new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    });
            }

            if (ImageList != null)
            {
                int index = TabPages[i].ImageIndex;
                if (index > -1)
                {
                    Image image = ImageList.Images[index];
                    graphics.DrawImage(image, new Point(tabRect.Location.X + 8, tabRect.Location.Y + image.Height / 2));
                }
            }

            e.Graphics.DrawImage(bitmap, Point.Empty);
        }
    }

    /// <summary>
    ///     Raises the <see cref="TabControl.Selecting" /> event.
    /// </summary>
    /// <param name="e">
    ///     A <see cref="TabControlCancelEventArgs" /> that contains the event data.
    /// </param>
    protected override void OnSelecting(TabControlCancelEventArgs e)
    {
        try
        {
            if (TransitionEnabled)
            {
                if (_oldIndex < e.TabPageIndex)
                {
                    TransitionRight(TabPages[_oldIndex], TabPages[e.TabPageIndex]);
                }
                else
                {
                    TransitionLeft(TabPages[_oldIndex], TabPages[e.TabPageIndex]);
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    ///     Raises the <see cref="TabControl.Deselecting" /> event.
    /// </summary>
    /// <param name="e">
    ///     A <see cref="TabControlCancelEventArgs" /> that contains the event data
    /// </param>
    protected override void OnDeselecting(TabControlCancelEventArgs e)
    {
        _oldIndex = e.TabPageIndex;
    }

    /// <summary>
    ///     Slides right from one <see cref="TabPage" /> to another.
    /// </summary>
    /// <param name="firstTabPage">The <see cref="TabPage" /> to slide from. </param>
    /// <param name="secondTabPage">The <see cref="TabPage" /> to slide to.</param>
    private void TransitionRight(TabPage firstTabPage, TabPage secondTabPage)
    {
        Graphics graphics = firstTabPage.CreateGraphics();

        var firstTabBitmap = new Bitmap(firstTabPage.Width, firstTabPage.Height);
        var secondTabBitmap = new Bitmap(secondTabPage.Width, secondTabPage.Height);

        firstTabPage.DrawToBitmap(firstTabBitmap,
            new Rectangle(Point.Empty, new Size(firstTabPage.Width, firstTabPage.Height)));
        secondTabPage.DrawToBitmap(secondTabBitmap,
            new Rectangle(Point.Empty, new Size(secondTabPage.Width, secondTabPage.Height)));

        foreach (Control control in firstTabPage.Controls)
            control.Hide();

        int slide = firstTabPage.Width - (firstTabPage.Width % _transitionSpeed);

        int a;
        for (a = 0; a >= -slide; a += -_transitionSpeed)
        {
            graphics.DrawImage(firstTabBitmap,
                new Rectangle(new Point(a, 0), new Size(firstTabPage.Width, firstTabPage.Height)));
            graphics.DrawImage(secondTabBitmap,
                new Rectangle(new Point(a + secondTabPage.Width, 0), new Size(secondTabPage.Width, secondTabPage.Height)));
        }
        a = firstTabPage.Width;

        graphics.DrawImage(firstTabBitmap,
            new Rectangle(new Point(a, 0), new Size(firstTabPage.Width, firstTabPage.Height)));
        graphics.DrawImage(secondTabBitmap,
            new Rectangle(new Point(a + secondTabPage.Width, 0), new Size(secondTabPage.Width, secondTabPage.Height)));

        SelectedTab = secondTabPage;

        foreach (Control control in secondTabPage.Controls)
            control.Show();

        foreach (Control control in firstTabPage.Controls)
            control.Show();

        graphics.Dispose();
        firstTabBitmap.Dispose();
        secondTabBitmap.Dispose();
    }

    /// <summary>
    ///     Slides left from one <see cref="TabPage" /> to another.
    /// </summary>
    /// <param name="firstTabPage">The <see cref="TabPage" /> to slide from. </param>
    /// <param name="secondTabPage">The <see cref="TabPage" /> to slide to.</param>
    private void TransitionLeft(TabPage firstTabPage, TabPage secondTabPage)
    {
        Graphics graphics = firstTabPage.CreateGraphics();

        var firstTabBitmap = new Bitmap(firstTabPage.Width, firstTabPage.Height);
        var secondTabBitmap = new Bitmap(secondTabPage.Width, secondTabPage.Height);

        firstTabPage.DrawToBitmap(firstTabBitmap,
            new Rectangle(Point.Empty, new Size(firstTabPage.Width, firstTabPage.Height)));
        secondTabPage.DrawToBitmap(secondTabBitmap,
            new Rectangle(Point.Empty, new Size(secondTabPage.Width, secondTabPage.Height)));

        foreach (Control control in firstTabPage.Controls)
            control.Hide();

        int slide = firstTabPage.Width - (firstTabPage.Width % _transitionSpeed);

        int a;
        for (a = 0; a <= slide; a += _transitionSpeed)
        {
            graphics.DrawImage(firstTabBitmap,
                new Rectangle(new Point(a, 0), new Size(firstTabPage.Width, firstTabPage.Height)));
            graphics.DrawImage(secondTabBitmap,
                new Rectangle(new Point(a - secondTabPage.Width, 0), new Size(secondTabPage.Width, secondTabPage.Height)));
        }
        a = firstTabPage.Width;

        graphics.DrawImage(firstTabBitmap,
            new Rectangle(new Point(a, 0), new Size(firstTabPage.Width, firstTabPage.Height)));
        graphics.DrawImage(secondTabBitmap,
            new Rectangle(new Point(a - secondTabPage.Width, 0), new Size(secondTabPage.Width, secondTabPage.Height)));

        SelectedTab = secondTabPage;

        foreach (Control control in secondTabPage.Controls)
            control.Show();

        foreach (Control control in firstTabPage.Controls)
            control.Show();

        graphics.Dispose();
        firstTabBitmap.Dispose();
        secondTabBitmap.Dispose();
    }
}

internal class TUPSTabcontrolX : TabControl
{
    // Fields
    private readonly Pen Border;

    // Methods
    public TUPSTabcontrolX()
    {
        Border = Pens.Black;
        SetStyle(
            ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint, true);
        DoubleBuffered = true;
        SizeMode = TabSizeMode.Fixed;
        var size = new Size(0x2c, 0x88);
        ItemSize = size;
    }


    protected override void CreateHandle()
    {
        base.CreateHandle();
        Alignment = TabAlignment.Left;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Rectangle tabRect;
        Point point4;
        Point point5;
        Size size3;
        StringFormat format;
        var image = new Bitmap(Width, Height);
        Graphics graphics = Graphics.FromImage(image);
        try
        {
            SelectedTab.BackColor = Color.FromArgb(30, 30, 30);
        }
        catch
        {
        }
        graphics.Clear(Color.FromArgb(30, 30, 30));
        var point = new Point(ItemSize.Height + 3, 0);
        var location = new Point(ItemSize.Height + 3, 0x3e7);
        graphics.DrawLine(Border, point, location);
        Size itemSize = ItemSize;
        location = new Point(itemSize.Height + 2, 0);
        point = new Point(ItemSize.Height + 2, 0x3e7);
        graphics.DrawLine(new Pen(Color.FromArgb(15, Color.White)), location, point);
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        graphics.DrawRectangle(Border, rect);
        rect = new Rectangle(1, 1, Width - 3, Height - 3);
        graphics.DrawRectangle(new Pen(Color.FromArgb(15, Color.White)), rect);
        int num = TabCount - 1;
        int index = 0;
    Label_0147:
        if (index > num)
        {
            e.Graphics.DrawImage((Image)image.Clone(), 0, 0);
            graphics.Dispose();
            image.Dispose();
            return;
        }
        if (index == SelectedIndex)
        {
            Rectangle rectangle2;
            Point point3;
            if (index == -1)
            {
                point = GetTabRect(index).Location;
                point3 = new Point(GetTabRect(index).Location.X - 2, point.Y - 2);
                itemSize = new Size(GetTabRect(index).Width + 3, GetTabRect(index).Height + 1);
                rectangle2 = new Rectangle(point3, itemSize);
            }
            else
            {
                tabRect = GetTabRect(index);
                point = new Point(tabRect.Location.X - 2, GetTabRect(index).Location.Y - 2);
                itemSize = new Size(GetTabRect(index).Width + 3, GetTabRect(index).Height);
                rectangle2 = new Rectangle(point, itemSize);
            }
            var blend = new ColorBlend();
            blend.Colors = new[] { Color.FromArgb(195, 90, 70), Color.FromArgb(30, 30, 30), Color.FromArgb(20, 20, 20) };
            blend.Positions = new[] { 0f, 0.5f, 1f };
            var brush = new LinearGradientBrush(rectangle2, Color.Black, Color.Black, 90f)
            {
                InterpolationColors = blend
            };
            graphics.FillRectangle(brush, rectangle2);
            graphics.DrawRectangle(Border, rectangle2);
            tabRect = new Rectangle(rectangle2.Location.X + 1, rectangle2.Location.Y + 1, rectangle2.Width - 2,
                rectangle2.Height - 2);
            graphics.DrawRectangle(new Pen(Color.FromArgb(15, Color.White)), tabRect);
            location = GetTabRect(index).Location;
            point = new Point(GetTabRect(index).Location.X - 2, location.Y - 2);
            itemSize = new Size(GetTabRect(index).Width + 3, GetTabRect(index).Height + 1);
            rectangle2 = new Rectangle(point, itemSize);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            var pointArray = new Point[3];
            itemSize = ItemSize;
            tabRect = GetTabRect(index);
            point3 = tabRect.Location;
            location = new Point(itemSize.Height - 3, point3.Y + 20);
            pointArray[0] = location;
            point = GetTabRect(index).Location;
            point4 = new Point(ItemSize.Height + 4, point.Y + 14);
            pointArray[1] = point4;
            size3 = ItemSize;
            point5 = new Point(size3.Height + 4, GetTabRect(index).Location.Y + 0x1b);
            pointArray[2] = point5;
            Point[] points = pointArray;
            graphics.DrawPolygon(new Pen(Color.FromArgb(15, Color.White), 3f), points);
            graphics.FillPolygon(new SolidBrush(Color.FromArgb(30, 30, 30)), points);
            graphics.DrawPolygon(Border, points);
            if (ImageList != null)
            {
                try
                {
                    if (ImageList.Images[TabPages[index].ImageIndex] != null)
                    {
                        point5 = rectangle2.Location;
                        point4 = new Point(point5.X + 8, rectangle2.Location.Y + 6);
                        graphics.DrawImage(ImageList.Images[TabPages[index].ImageIndex], point4);
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString("      " + TabPages[index].Text, Font, Brushes.DimGray, rectangle2, format);
                    }
                    else
                    {
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString(TabPages[index].Text, Font, Brushes.White, rectangle2, format);
                    }
                    goto Label_09D5;
                }
                catch
                {
                    format = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };
                    graphics.DrawString(TabPages[index].Text, Font, Brushes.White, rectangle2, format);
                    goto Label_09D5;
                }
            }
            format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            graphics.DrawString(TabPages[index].Text, Font, Brushes.White, rectangle2, format);
        }
        else
        {
            tabRect = GetTabRect(index);
            point5 = tabRect.Location;
            Point point6 = GetTabRect(index).Location;
            point4 = new Point(point5.X - 1, point6.Y - 1);
            size3 = new Size(GetTabRect(index).Width + 2, GetTabRect(index).Height);
            var layoutRectangle = new Rectangle(point4, size3);
            point5 = new Point(layoutRectangle.Right, layoutRectangle.Top);
            point6 = new Point(layoutRectangle.Right, layoutRectangle.Bottom);
            graphics.DrawLine(Border, point5, point6);
            point5 = new Point(layoutRectangle.Right - 1, layoutRectangle.Top);
            point6 = new Point(layoutRectangle.Right - 1, layoutRectangle.Bottom);
            graphics.DrawLine(new Pen(Color.FromArgb(0x2b, 0x2b, 0x2b)), point5, point6);
            if (ImageList != null)
            {
                try
                {
                    if (ImageList.Images[TabPages[index].ImageIndex] != null)
                    {
                        point5 = layoutRectangle.Location;
                        point4 = new Point(point5.X + 8, layoutRectangle.Location.Y + 6);
                        graphics.DrawImage(ImageList.Images[TabPages[index].ImageIndex], point4);
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString("      " + TabPages[index].Text, Font,
                            new SolidBrush(Color.FromArgb(170, 170, 170)), layoutRectangle, format);
                    }
                    else
                    {
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString(TabPages[index].Text, Font, new SolidBrush(Color.FromArgb(170, 170, 170)),
                            layoutRectangle, format);
                    }
                    goto Label_09D5;
                }
                catch
                {
                    format = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };
                    graphics.DrawString(TabPages[index].Text, Font, new SolidBrush(Color.FromArgb(170, 170, 170)),
                        layoutRectangle, format);
                    goto Label_09D5;
                }
            }
            format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            graphics.DrawString(TabPages[index].Text, Font, new SolidBrush(Color.FromArgb(170, 170, 170)),
                layoutRectangle, format);
        }
    Label_09D5:
        index++;
        goto Label_0147;
    }

    public Brush ToBrush(Color color)
    {
        return new SolidBrush(color);
    }

    public Pen ToPen(Color color)
    {
        return new Pen(color);
    }
}

[DefaultEvent("TextChanged")]
internal class TUPSTextBox : ThemeControl154
{
    // Fields
    private readonly TextBox Base = new TextBox();
    private Color Background;
    private Color Border;
    private int _MaxLength = 0x7fff;
    private bool _Multiline;
    private bool _ReadOnly;
    private HorizontalAlignment _TextAlign = HorizontalAlignment.Left;
    private bool _UseSystemPasswordChar;

    // Methods
    public TUPSTextBox()
    {
        Base.Font = Font;
        Base.Text = Text;
        Base.MaxLength = _MaxLength;
        Base.Multiline = _Multiline;
        Base.ReadOnly = _ReadOnly;
        Base.UseSystemPasswordChar = _UseSystemPasswordChar;
        Base.BorderStyle = BorderStyle.None;
        var point = new Point(4, 4);
        Base.Location = point;
        Base.Width = Width - 10;
        if (_Multiline)
        {
            Base.Height = Height - 11;
        }
        else
        {
            LockHeight = Base.Height + 11;
        }
        Base.TextChanged += OnBaseTextChanged;
        Base.KeyDown += OnBaseKeyDown;
        SetColor("Text", 170, 170, 170);
        SetColor("Background", 0x16, 0x16, 0x16);
        SetColor("Border", 0, 0, 0);
    }

    // Properties
    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            if (Base != null)
            {
                Base.Font = value;
                var point = new Point(3, 5);
                Base.Location = point;
                Base.Width = Width - 6;
                if (!_Multiline)
                {
                    LockHeight = Base.Height + 11;
                }
            }
        }
    }

    public int MaxLength
    {
        get { return _MaxLength; }
        set
        {
            _MaxLength = value;
            if (Base != null)
            {
                Base.MaxLength = value;
            }
        }
    }

    public bool Multiline
    {
        get { return _Multiline; }
        set
        {
            _Multiline = value;
            if (Base != null)
            {
                Base.Multiline = value;
                if (value)
                {
                    LockHeight = 0;
                    Base.Height = Height - 11;
                }
                else
                {
                    LockHeight = Base.Height + 11;
                }
            }
        }
    }

    public string PasswordChar
    {
        get { return Base.PasswordChar.ToString(); }
        set { Base.PasswordChar = (value).ToCharArray()[0]; }
    }

    public bool ReadOnly
    {
        get { return _ReadOnly; }
        set
        {
            _ReadOnly = value;
            if (Base != null)
            {
                Base.ReadOnly = value;
            }
        }
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            if (Base != null)
            {
                Base.Text = value;
            }
        }
    }

    public HorizontalAlignment TextAlign
    {
        get { return _TextAlign; }
        set
        {
            _TextAlign = value;
            if (Base != null)
            {
                Base.TextAlign = value;
            }
        }
    }

    public bool UseSystemPasswordChar
    {
        get { return _UseSystemPasswordChar; }
        set
        {
            _UseSystemPasswordChar = value;
            if (Base != null)
            {
                Base.UseSystemPasswordChar = value;
            }
        }
    }

    protected override void ColorHook()
    {
        Background = GetColor("Background");
        Border = GetColor("Border");
        Base.ForeColor = GetColor("Text");
        Base.BackColor = Background;
    }

    private void OnBaseKeyDown(object sender, KeyEventArgs e)
    {
        if (((!e.Control || (e.KeyCode != Keys.A)) ? 0 : 1) != 0)
        {
            Base.SelectAll();
            e.SuppressKeyPress = true;
        }
    }

    private void OnBaseTextChanged(object sender, EventArgs e)
    {
        Text = Base.Text;
    }

    protected override void OnCreation()
    {
        if (!Controls.Contains(Base))
        {
            Controls.Add(Base);
        }
    }

    protected override void OnResize(EventArgs e)
    {
        var point = new Point(5, 5);
        Base.Location = point;
        Base.Width = Width - 10;
        if (_Multiline)
        {
            Base.Height = Height - 5;
        }
        base.OnResize(e);
    }

    protected override void PaintHook()
    {
        base.G.Clear(BackColor);
        base.G.SmoothingMode = SmoothingMode.HighQuality;
        base.G.FillPath(new SolidBrush(Background), CreateRound(0, 0, Width - 1, Height - 1, 6));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White)), CreateRound(1, 1, Width - 3, Height - 3, 6));
        base.G.DrawPath(new Pen(Border), CreateRound(0, 0, Width - 1, Height - 1, 6));
    }
}

#endregion

#region Space

public class SpaceForm : Form
{
    public SpaceForm()
    {
        SetStyle(ControlStyles.UserPaint, true);
        DoubleBuffered = true;

        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Verdana", 8);
        Size = new Size(550, 400);
        ShowIcon = false;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.FillRectangle(DrawX.Grain, ClientRectangle);
        base.OnPaint(e);
    }
}

public class SpaceDivider : Panel
{
    #region Properties

    private CState DividerState_ = CState.Horizontal;

    public CState DividerState
    {
        get { return DividerState_; }
        set
        {
            int OldW = Size.Width, OldH = Size.Height;
            DividerState_ = value;
            Size = new Size(DividerState_ == CState.Horizontal ? OldH : 4, DividerState_ == CState.Horizontal ? 4 : OldW);
            Invalidate();
        }
    }

    #endregion

    public SpaceDivider()
    {
        SetStyle((ControlStyles)2050, true);
        DoubleBuffered = true;

        BackColor = Color.Transparent;
        BorderStyle = BorderStyle.None;
        Size = new Size(40, 4);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.DrawLine(new Pen(Color.FromArgb(200, 10, 10, 10)), 1, 1,
            DividerState_ == CState.Horizontal ? Width - 2 : 1, DividerState_ == CState.Horizontal ? 1 : Height - 2);
        e.Graphics.DrawLine(new Pen(Color.FromArgb(200, 40, 40, 40)), DividerState_ == CState.Horizontal ? 1 : 2,
            DividerState_ == CState.Horizontal ? 2 : 1, DividerState_ == CState.Horizontal ? Width - 2 : 2,
            DividerState_ == CState.Horizontal ? 2 : Height - 2);
        base.OnPaint(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        base.SetBoundsCore(x, y, DividerState_ == CState.Horizontal ? width : 4,
            DividerState_ == CState.Horizontal ? 4 : height, specified);
    }
}

public class SpaceButton : Control
{
    private readonly Color[] DefColors = new Color[3] { Color.FromArgb(150, 0, 0, 0), Color.FromArgb(100, 24, 66, 122), Color.FromArgb(80, 156, 0, 0) };

    #region Properties

    private CColor ButtonColor_ = CColor.Blue;
    private int ButtonCurve_ = 5;
    private CStyle ButtonStyle_ = CStyle.Round;
    private MState MouseState = MState.None;

    [Browsable(false)]
    public override Color BackColor
    {
        get { return base.BackColor; }
        set { base.BackColor = value; }
    }

    [Browsable(false)]
    public override Color ForeColor
    {
        get { return base.ForeColor; }
        set { base.ForeColor = value; }
    }

    [Browsable(false)]
    public override Image BackgroundImage
    {
        get { return base.BackgroundImage; }
        set { base.BackgroundImage = value; }
    }

    [Browsable(false)]
    public override ImageLayout BackgroundImageLayout
    {
        get { return base.BackgroundImageLayout; }
        set { base.BackgroundImageLayout = value; }
    }

    [DefaultValue(typeof(Font), "Verdana, 8")]
    public override Font Font
    {
        get { return base.Font; }
        set { base.Font = value; }
    }

    [Description("The curve in-which the button will draw it's edges.")]
    public int ButtonCurve
    {
        get { return ButtonCurve_; }
        set
        {
            ButtonCurve_ = value >= 0 ? value : 0;
            Invalidate();
        }
    }

    [Description("The color of the button ranging from ~\nBlack(0, 0, 0), Blue(24, 66, 122), and Red(156, 0, 0).")]
    public CColor ButtonColor
    {
        get { return ButtonColor_; }
        set
        {
            ButtonColor_ = value;
            Invalidate();
        }
    }

    [Description("This switch will override the 'ButtonCurve' property to allow, or disallow the changing of its value."
        )]
    public CStyle ButtonStyle
    {
        get { return ButtonStyle_; }
        set
        {
            ButtonStyle_ = value;
            ButtonCurve = value == CStyle.Square ? 0 : 5;
            Invalidate();
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        MouseState = MState.None;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        MouseState = MState.Over;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            MouseState = MState.Over;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            MouseState = MState.Down;
        Invalidate();
    }

    #endregion

    public SpaceButton()
    {
        SetStyle((ControlStyles)2050, true);
        DoubleBuffered = true;

        Font = new Font("Verdana", 8);
        Size = new Size(115, 23);
        BackColor = Color.Transparent;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var CR = new Rectangle(0, 0, Width - 1, Height - 1);
        var IR = new Rectangle(1, 1, Width - 3, Height - 3);
        var TR = new Rectangle(0, 0, Width, Height + 1);
        var TRS = new Rectangle(0, 0, Width + 2, Height + 2);
        var Outline =
            new Pen(
                Color.FromArgb(
                    (int)ButtonColor == 0 ? ((int)MouseState == 1 ? 13 : 11) : ((int)MouseState == 1 ? 18 : 14),
                    Color.White));

        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        e.Graphics.FillPath(DrawX.Grain, DrawX.Round(CR, ButtonCurve));

        if ((int)ButtonColor == 0)
            e.Graphics.FillPath(
                DrawX.Gradient(Color.FromArgb((int)MouseState == 2 ? 100 : 50, Color.Black),
                    Color.FromArgb((int)MouseState == 2 ? 70 : 100, Color.Black), CR, 90f),
                DrawX.Round(CR, ButtonCurve));
        else
        {
            e.Graphics.FillPath(new SolidBrush(DefColors[(int)ButtonColor]), DrawX.Round(CR, ButtonCurve));
            e.Graphics.FillPath(
                DrawX.Gradient((int)MouseState == 2 ? Color.FromArgb(50, Color.Black) : Color.Transparent,
                    (int)MouseState == 2 ? Color.Transparent : Color.FromArgb(50, Color.Black), CR, 90f),
                DrawX.Round(CR, ButtonCurve));
        }

        e.Graphics.DrawPath(new Pen(Color.FromArgb(250, 15, 15, 15)), DrawX.Round(CR, ButtonCurve));
        e.Graphics.DrawPath(Outline, DrawX.Round(IR, ButtonCurve));

        e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(120, Color.Black)), TRS,
            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(220, 225, 225, 225)), TR,
            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

        if (!Enabled)
            e.Graphics.FillPath(new SolidBrush(Color.FromArgb(100, Color.Black)), DrawX.Round(CR, ButtonCurve));

        base.OnPaint(e);
    }

    protected override void OnTextChanged(EventArgs e)
    {
        Invalidate();
        base.OnTextChanged(e);
    }
}

[DefaultEvent("CheckedChanged")]
public class SpaceCheckBox : Control
{
    private readonly Color[] DefColors = new Color[3] { Color.FromArgb(0, 0, 0), Color.FromArgb(24, 66, 122), Color.FromArgb(156, 0, 0) };

    #region Events

    public delegate void CheckedChangedEventHandler(object sender, bool isChecked);

    public event CheckedChangedEventHandler CheckedChanged;

    #endregion

    #region Properties

    private CColor CheckColor_ = CColor.Blue;
    private bool Checked_;
    private MState MouseState = MState.None;

    [Description("The color of the check mark ranging from ~\nBlack(0, 0, 0) Blue(24, 66, 122), and Red(156, 0, 0).")]
    public CColor CheckColor
    {
        get { return CheckColor_; }
        set
        {
            CheckColor_ = value;
            Invalidate();
        }
    }

    [Description("Indicates whether the component is in the checked state.")]
    public bool Checked
    {
        get { return Checked_; }
        set
        {
            Checked_ = value;
            Invalidate();
            if (CheckedChanged != null) CheckedChanged(this, value);
        }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        MouseState = MState.None;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        MouseState = MState.Over;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            MouseState = MState.Over;
        Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            MouseState = MState.Down;
        Invalidate();
    }

    #endregion

    public SpaceCheckBox()
    {
        SetStyle((ControlStyles)2050, true);
        DoubleBuffered = true;
        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(220, 225, 225, 225);
        Font = new Font("Verdana", 8);
        Size = new Size(135, 16);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var SF = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        var CB = new Rectangle(0, 0, 16, 16);
        var IR = new Rectangle(1, 1, 13, 13);
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        e.Graphics.FillRectangle(DrawX.Gradient(Color.FromArgb(100, Color.Black), Color.Transparent, IR, 45f), IR);
        DrawX.Borders(e.Graphics, new Pen(Color.FromArgb(250, 15, 15, 15)), CB, 0);
        DrawX.Borders(e.Graphics, new Pen(Color.FromArgb((int)MouseState == 1 ? 10 : 8, Color.White)), CB, 1);
        DrawX.Borders(e.Graphics, new Pen(Color.FromArgb(150, 15, 15, 15)), CB, 2);
        if (Checked)
            e.Graphics.DrawString("v", new Font("Verdana", 8, FontStyle.Bold),
                new SolidBrush(DefColors[(int)CheckColor]), 1, 1);
        e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(100, Color.Black)), 20, 2);
        e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb(220, 225, 225, 225)), 18, 1);
        base.OnPaint(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        base.SetBoundsCore(x, y, 22 + (int)CreateGraphics().MeasureString(Text, Font).Width, 16, specified);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            Checked = !Checked;
        base.OnMouseClick(e);
    }

    protected override void OnTextChanged(EventArgs e)
    {
        Width = 22 + (int)CreateGraphics().MeasureString(Text, Font).Width;
        base.OnTextChanged(e);
    }
}

[DefaultEvent("TextChanged")]
public class SpaceTextBox : Control
{
    #region Properties

    private int Curve_ = 1;
    private int LockHeight_;
    private int MaxLength_ = int.MaxValue;
    private bool Multiline_;
    private bool ReadOnly_;
    private HorizontalAlignment TextAlign_ = HorizontalAlignment.Left;
    private bool UseSystemPasswordChar_;

    public string[] Lines
    {
        get { return Base != null ? Base.Lines : new[] { "<Empty>" }; }
        set { if (Base != null) Base.Lines = value; }
    }

    public int Curve
    {
        get { return Curve_; }
        set
        {
            Curve_ = value >= 0 ? value : 1;
            Invalidate();
        }
    }

    private int LockHeight
    {
        get { return LockHeight_; }
        set
        {
            LockHeight_ = value;
            if (!(LockHeight == 0) && IsHandleCreated)
                Height = LockHeight;
        }
    }

    public bool ReadOnly
    {
        get { return ReadOnly_; }
        set
        {
            ReadOnly_ = value;
            if (Base != null) Base.ReadOnly = value;
            Invalidate();
        }
    }

    public bool Multiline
    {
        get { return Multiline_; }
        set
        {
            Multiline_ = value;
            if (Base != null)
            {
                Base.Multiline = value;
                if (value)
                {
                    LockHeight = 0;
                    Base.Height = Height - 7;
                }
                else
                {
                    LockHeight = Base.Height + 7;
                }
            }
            Invalidate();
        }
    }

    public int MaxLength
    {
        get { return MaxLength_; }
        set
        {
            MaxLength_ = value;
            if (Base != null) Base.MaxLength = value;
            Invalidate();
        }
    }

    public bool UseSystemPasswordChar
    {
        get { return UseSystemPasswordChar_; }
        set
        {
            UseSystemPasswordChar_ = value;
            if (Base != null) Base.UseSystemPasswordChar = value;
            Invalidate();
        }
    }

    public HorizontalAlignment TextAlign
    {
        get { return TextAlign_; }
        set
        {
            TextAlign_ = value;
            if (Base != null) Base.TextAlign = value;
            Invalidate();
        }
    }

    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            if (Base != null) Base.Font = value;
            Invalidate();
        }
    }

    public override string Text
    {
        get { return Base != null ? Base.Text : ""; }
        set
        {
            base.Text = value;
            if (Base != null) Base.Text = value;
            Invalidate();
        }
    }

    public override Cursor Cursor
    {
        get { return Cursors.IBeam; }
        set { base.Cursor = Cursors.IBeam; }
    }

    #endregion

    #region Functions

    public void ScrollToCaret()
    {
        if (Base != null) Base.ScrollToCaret();
    }

    #endregion

    public TextBox Base;

    public SpaceTextBox()
    {
        Base = new TextBox();
        SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
        Font = new Font("Verdana", 8);

        #region Base Properties

        Base.BackColor = Color.FromArgb(22, 22, 22);
        Base.ForeColor = Color.FromArgb(225, 225, 225);
        Base.Font = Font;
        Base.Text = Text;
        Base.MaxLength = MaxLength_;
        Base.Multiline = Multiline_;
        Base.ReadOnly = ReadOnly_;
        Base.TextAlign = TextAlign_;
        Base.UseSystemPasswordChar = UseSystemPasswordChar_;
        Base.BorderStyle = BorderStyle.None;
        Base.Location = new Point(5, 3);
        Base.Width = Width - 10;
        ClientSize = new Size(156, 16);
        if (Multiline) Base.Height = Height - 7;
        else LockHeight = Base.Height + 7;

        #endregion

        Invalidate();
    }

    protected override void OnCreateControl()
    {
        if (!Controls.Contains(Base)) Controls.Add(Base);
        base.OnCreateControl();
    }

    protected override void OnResize(EventArgs e)
    {
        Base.Location = new Point(5, 3);
        Base.Width = Width - 10;
        if (Multiline) Base.Height = Height - 7;
        else LockHeight = Base.Height + 7;
        base.OnResize(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var CR = new Rectangle(0, 0, Width - 1, Height - 1);
        var IR = new Rectangle(1, 1, Width - 3, Height - 3);
        var IIR = new Rectangle(2, 2, Width - 5, Height - 5);
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);

        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;

        G.FillPath(new SolidBrush(Base.BackColor), DrawX.Round(CR, Curve));
        G.DrawPath(new Pen(Color.FromArgb(250, 15, 15, 15)), DrawX.Round(CR, Curve));
        G.DrawPath(new Pen(Color.FromArgb(4, Color.White)), DrawX.Round(IR, Curve));

        e.Graphics.DrawImage(B, 0, 0);
        G.Dispose();
        B.Dispose();
        base.OnPaint(e);
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        if (!(LockHeight_ == 0))
            Height = LockHeight_;
        base.OnHandleCreated(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
        if (!(LockHeight_ == 0))
            height = LockHeight_;
        base.SetBoundsCore(x, y, width, height, specified);
    }
}

public class SpaceComboBox : ComboBox
{
    #region Properties

    private CStyle ControlStyle_;
    private MState CursorState_ = MState.None;

    public CStyle ControlStyle
    {
        get { return ControlStyle_; }
        set
        {
            ControlStyle_ = value;
            Invalidate();
        }
    }

    public ComboBoxStyle DropDownStyle
    {
        get { return ComboBoxStyle.DropDownList; }
        set { base.DropDownStyle = ComboBoxStyle.DropDownList; }
    }

    public DrawMode DrawMode
    {
        get { return DrawMode.OwnerDrawFixed; }
        set { base.DrawMode = DrawMode.OwnerDrawFixed; }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        CursorState_ = MState.None;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        CursorState_ = MState.Over;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            CursorState_ = MState.Over;
        Invalidate();
        base.OnMouseUp(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            CursorState_ = MState.Down;
        Invalidate();
        base.OnMouseDown(e);
    }

    #endregion

    public SpaceComboBox()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        DrawMode = DrawMode.OwnerDrawFixed;
        DropDownStyle = ComboBoxStyle.DropDownList;
        ControlStyle = CStyle.Round;
        ForeColor = Color.FromArgb(225, 225, 225);
        Font = new Font("Verdana", 8);
        BackColor = Color.Transparent;
        ItemHeight = 17;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        var SH = (int)G.MeasureString("...", Font).Height;
        bool isOver = CursorState_ == MState.Over ? true : false;
        bool isDown = CursorState_ == MState.Down ? true : false;
        bool isRound = ControlStyle == CStyle.Round ? true : false;
        var CR = new Rectangle(0, 0, Width - 1, Height - 1);
        var IR = new Rectangle(1, 1, Width - 3, Height - 3);
        var RB = new Rectangle(Width - 23, 1, 21, Height - 3);
        var RO = new Rectangle(Width - 24, 1, 22, Height - 3);
        LinearGradientBrush Gradient = DrawX.Gradient(Color.FromArgb(50, Color.Black), Color.FromArgb(100, Color.Black),
            CR, 90f);
        G.Clear(BackColor);
        G.SmoothingMode = SmoothingMode.HighQuality;

        G.FillPath(DrawX.Grain, DrawX.Round(CR, isRound ? 5 : 0));
        G.FillPath(Gradient, DrawX.Round(CR, isRound ? 5 : 0));
        G.DrawPath(new Pen(Color.FromArgb(250, 15, 15, 15)), DrawX.Round(CR, isRound ? 5 : 0));
        G.DrawPath(new Pen(Color.FromArgb(isOver ? 11 : 9, Color.White)), DrawX.Round(IR, isRound ? 5 : 0));

        G.DrawLine(new Pen(Color.FromArgb(isOver ? 11 : 9, Color.White)), Width - 26, 2, Width - 26, Height - 3);
        G.DrawLine(new Pen(Color.FromArgb(250, 15, 15, 15)), Width - 27, 2, Width - 27, Height - 3);

        G.DrawString(
            SelectedIndex != -1
                ? " " + Items[SelectedIndex]
                : Items != null && Items.Count > 0 ? " " + Items[0] : " <Empty>", Font,
            new SolidBrush(Color.FromArgb(120, Color.Black)), 5, ((Height / 2) - ((SH + 1) / 2)) + 1);
        G.DrawString(
            SelectedIndex != -1
                ? " " + Items[SelectedIndex]
                : Items != null && Items.Count > 0 ? " " + Items[0] : " <Empty>", Font,
            new SolidBrush(Color.FromArgb(200, 225, 225, 225)), 4, (Height / 2) - ((SH + 1) / 2));

        G.FillPolygon(new SolidBrush(Color.FromArgb(120, Color.Black)),
            DrawX.Triangle(new Point(Width - 17, Height / 2), new Size(7, 3)));
        G.FillPolygon(new SolidBrush(Color.FromArgb(isOver ? 240 : 220, 225, 225, 225)),
            DrawX.Triangle(new Point(Width - 18, (Height / 2) - 1), new Size(7, 3)));

        e.Graphics.DrawImage(B, 0, 0);
        G.Dispose();
        B.Dispose();
        base.OnPaint(e);
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        bool isRound = ControlStyle == CStyle.Round ? true : false;
        if (e.Index < 0) return;
        if ((int)e.State == 785 | (int)e.State == 17 | e.State == DrawItemState.Focus |
            e.State == DrawItemState.HotLight | e.State == DrawItemState.Selected)
        {
            var SelRec = new Rectangle(new Point(e.Bounds.X, e.Bounds.Y),
                new Size(e.Bounds.Width - 1, e.Bounds.Height - 1));
            e.Graphics.FillRectangle(DrawX.Grain, e.Bounds);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Black)), e.Bounds);

            e.Graphics.FillPath(new SolidBrush(Color.FromArgb(120, Color.Black)), DrawX.Round(SelRec, isRound ? 4 : 0));
            e.Graphics.DrawPath(new Pen(Color.FromArgb(40, 50, 50, 50)), DrawX.Round(SelRec, isRound ? 4 : 0));
            e.Graphics.DrawString("  " + Items[e.Index], Font, new SolidBrush(Color.FromArgb(225, 225, 225)), e.Bounds.X,
                e.Bounds.Y + 1);
        }
        else
        {
            e.Graphics.FillRectangle(DrawX.Grain, e.Bounds);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Black)), e.Bounds);
            e.Graphics.DrawString(" " + Items[e.Index], Font, new SolidBrush(Color.FromArgb(225, 225, 225)), e.Bounds.X,
                e.Bounds.Y + 1);
        }
        base.OnDrawItem(e);
    }
}

public class SpaceTabControl : TabControl
{
    private Color TabHighlight_ = Color.FromArgb(36, 75, 140);

    public SpaceTabControl()
    {
        SetStyle((ControlStyles)2050, true);
        DoubleBuffered = true;
    }

    [DefaultValue(typeof(Color), "36, 75, 140")]
    public Color TabHighlight
    {
        get { return TabHighlight_; }
        set
        {
            TabHighlight_ = value.A == 255 ? value : Color.FromArgb(value.R, value.G, value.B);
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var SF = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        e.Graphics.FillRectangle(DrawX.Grain, ClientRectangle);
        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Black)),
            new Rectangle(new Point(0, 0), new Size(Width, 21)));

        DrawX.Borders(e.Graphics, new Pen(Color.FromArgb(250, 15, 15, 15)),
            new Rectangle(new Point(0, 0), new Size(Width, 22)), 0);
        DrawX.Borders(e.Graphics, new Pen(Color.FromArgb(250, 15, 15, 15)),
            new Rectangle(new Point(0, 21), new Size(Width, Height - 21)), 0);
        DrawX.Borders(e.Graphics, new Pen(Color.FromArgb(9, Color.White)),
            new Rectangle(new Point(0, 21), new Size(Width, Height - 21)), 1);

        Rectangle TR;
        foreach (TabPage TP in TabPages)
        {
            if (TP.BackgroundImage != DrawX.GrainCode) TP.BackgroundImage = DrawX.GrainCode;
            if (TP.BackColor != Color.Black) TP.BackColor = Color.Black;
            TR = GetTabRect(TabPages.IndexOf(TP));
            if (TabPages.IndexOf(TP) == SelectedIndex)
            {
                e.Graphics.FillRectangle(DrawX.Grain, new Rectangle(TR.X - 1, TR.Y - 1, TR.Width + 1, TR.Height - 1));
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.Black)),
                    new Rectangle(TR.X - 1, TR.Y - 1, TR.Width + 1, TR.Height - 1));
                DrawX.Borders(e.Graphics, new Pen(Color.FromArgb(12, Color.FromArgb(15, 15, 15))),
                    new Rectangle(TR.X - 1, TR.Y - 1, TR.Width + 1, TR.Height - 1));

                switch (SelectedIndex < 1)
                {
                    case true:
                        DrawX.Gradient(e.Graphics, Color.Transparent, Color.FromArgb(255, TabHighlight), TR.Width + 1, 1,
                            1, (TR.Height / 2) - 1);
                        DrawX.Gradient(e.Graphics, Color.FromArgb(255, TabHighlight), Color.Transparent, TR.Width + 1,
                            TR.Height / 2, 1, (TR.Height / 2));
                        break;
                    case false:
                        DrawX.Gradient(e.Graphics, Color.Transparent, Color.FromArgb(255, TabHighlight),
                            new Rectangle(new Point(TR.X - 1, 1), new Size(1, (TR.Height / 2) - 1)));
                        DrawX.Gradient(e.Graphics, Color.FromArgb(255, TabHighlight), Color.Transparent,
                            new Rectangle(new Point(TR.X - 1, TR.Height / 2), new Size(1, TR.Height / 2)));
                        if (SelectedIndex == TabPages.Count - 1) break;
                        DrawX.Gradient(e.Graphics, Color.Transparent, Color.FromArgb(255, TabHighlight),
                            new Rectangle(new Point((TR.X + TR.Width) - 1, 1), new Size(1, (TR.Height / 2) - 1)));
                        DrawX.Gradient(e.Graphics, Color.FromArgb(255, TabHighlight), Color.Transparent,
                            new Rectangle(new Point((TR.X + TR.Width) - 1, TR.Height / 2), new Size(1, TR.Height / 2)));
                        break;
                }
            }
            if (TabPages.IndexOf(TP) == SelectedIndex)
                TR = new Rectangle(TR.X, TR.Y, TR.Width, TR.Height - 2);
            e.Graphics.DrawString(TP.Text, Font, new SolidBrush(Color.FromArgb(120, Color.Black)), TR, SF);
            e.Graphics.DrawString(TP.Text, Font, new SolidBrush(Color.FromArgb(220, 225, 225, 225)), TR, SF);
        }
    }
}

#endregion

#region NiceBright

//private Color Accent=Color.FromArgb(135,177,74);
//private Color Border = Color.FromArgb(225, 225, 225);
//private Color TextColor = Color.FromArgb(23, 23, 23);
//private Color TitleBottom=Color.FromArgb(170,170,170);
//private Color TitleTop=Color.FromArgb(245,245,245);
//private Color Base = Color.FromArgb(232, 232, 232);
public static class DrawGrayPattern
{
    #region Grain Texture

    public static Bitmap GrainCode =
        (Bitmap)
            (System.Drawing.Image.FromStream(
                new MemoryStream(
                    Convert.FromBase64String(
                        "iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAIAAAGIhzKVAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAJgNJREFUeNokkAcKhEAMRd04ViyIIt7/DJ5KFBF7ZZ8mIMT8kp/51XXd973jOLZtR1HUNE2apsMwlGV5nuc0TXme7/suwFmWua5rWdZ930yVxC96flHiYtZ1VQZFc10XU7AwDD3Pw5JfOAYsCAJIMDB4nqeqqhcwhqWgLDyOQ/ggKaMoCsLRiwgy3/fpt21jKIR7vkKADbvocWIJZtlXxDDc4n1Fw71gZALDiZ77xnGkETXjn+BEbtv23SKCjMb6Ko5jsywLCXDSKFDxA4PEdlDOYi9KAeAolep2YqCBxEN2Xcd9Ru+AijkAI31O5mjeZF+ZeZ71qcBwJZ+eye3oUZL7vRcupCRJmBJO35ZdwMi4j4dk+18AHukYh2EYhqEo6mbr/Q/jG3n2mqXP+UAMJHAiSqREfeaca62EGwHeDKUk62SOevbBe4i2I3HeGFzOBvyeA906oaheIzwGqiH8dpq/EKRgVx71Ee5vZvsFJ0YNBmEhHQxj9J19YhAS9Mt4AyPobJGwVmUIS8JO3/0cd9NyuTwQariwpZntve0Z4Ro8lToC8pqh7FgIgGbMADfiNimX2j6FKSltyHunUEfeYtqEIx/v9X1O2iGaQvosFseOGpWwANGIFIIajNlHjD9/AViqlxwAQRiKopG4/6WwNWfGoQcuDoyU9vX3Wq85Jw0BlmDeFY+qkDmpg5G7qXFs9gCximz1cIhVgd79tBhkDEgDgQqdTemRAxJ55OqqN9tVSt4ogYtmEUqD6qtbx0oCGlBbxFVMlMHhQgk2TAJucpCnIWsUGOBFI5mZY8PSMii0EZtpc9LwxnKM98EHvZZJO8nRVRuHPkQIEPk+SycR1daNY3KeyRnHAjYy0lAfTYCnTSu/1WxmVVRfffNMCFGM9Coit2Xnoam4LR7fa5J3A2/2bRRuGcCKMqgFTrErYn1sstpg0blJcoQ+BBxHqwvo9kakLjWtKFm31JIjcdlVrzUyBeKuWnq3B6SpBLy1L1BEOsKEGxVaC23gintSY8wzexTtl9WvooEnIQeEN2kSCrCVBeXZz90fDJzrbz+ylougIqS6qC6JiGJDRait9L0VF8gvANN1llsxEAJRVGn1/tfizeU/Fx8L5X0kHhgKGij88zxPysXYU0M//Lg3z8lpIFMYCfdK+tGtThzSNcawSff/O64nzOUp6eXSMFI2IXDLZFkp9aLsKuvQ5Va+p8vOcSHwTiMf5m9RZygfUnYLvrNTmaQVS7Z0Zc8zZ3xaB4y6wHaNn03xmYSSWocnXTXmJHSmbIhylq3eahSlIPYkI8jdMc5Sj47PaAr2mWn9czJqzbDFKKWuqxKnZKnpAK/hGSK9Vu4T1UDBRgBOw62WLrTyUOkXk6Lv4bV0qU+8uWtDsJG6YHGNVAKoziV6guh1/+BKyLvQKeBZIV5cwTfKbWoAmoI6bwaEui9HQ5P32lw8VB/YTwYgdeg9LKb86fDximwRqKNZ0tXnAtTtygUuE7yzYqjcXUvp7/uTQll3OoYqu6ab5cjIXorQABecRNVOTwtzA0y5BMlgtzqmJzuRmFN0V+2kmbl8dvZchdeSlQnW85omxhNyf0tWKl3POaaJ/pRJChnt9ttT31/6oFnQhJmDTKh7hXWZDK1NBl1mPaEkgubsi46PAsyKzSccHb3MGh7HZqarewcIV5kAuUD0cwJpWpCkAq7UPx5KwhgRr1XFetDPwqsmM5cDgzNJo62EZO5r3XDqAxnpdUAyEcDCRGU+VeS0MNFiPspS1seoDw7cp7g1BHYwoxNQU84xtS6SXEL4FvbM6xgR4XByaNXyajR3nS0B+sJg6NsN0jcbkuu+174CljIygR0oJFBLGYfAGn8zCJR1mnbB/KRsF0TjCSUTfIdjzvjGQCu98jlzttHA4aGL4uohhXywW6aDk9psNeckST10E1xRhLmrFKyGGUpBn1qeus2QiIDaAuqvb6d+fwIwZS+5CQQxEIZFZ5aRcjBuy8nYRVEW5O/5JhYsEAz9cLvtqrK5PR4PEMzZbSiI5A09Uez8nK/GEKT96vi5Eg23Fk5SRrm3BuMjPN3EzGoL1N6yoLDDFH/eHeNSo1xvTuPatRVJo/fDqT0b0LF+/1/KL0Kc2ICPrQD8R4nuYuI/vzoP1pJE0l1YbgUgI6CRUHE9w4jYnTClMpjbT8KspbOyMVmcQbJXsORIt5PDgh2GAiVyXYYBLwqlY7TLwUMiSByS7PC+nVoOkAh90DzF+aCr4jQrZ7yLpjOomAxVyzgDE7MsTOpr7zARaB19H2WsUSCARFvvnRX/KrOMHImYfRzJVuoCN0DYS8muqylh5Ywj81scR7kKkmrxUD+IcRiomG+OGHJoadJIHaBRhSR7v6ov1OACkWgxOCNyc9Yojzo/Vu6JhIdFLbJZ3bR8hl3QHORtkC6Gs+6m0xltDaZ98QWM9Ryr4t0uQaQ20oVgXhDQ9l2opMFKXRE83oncNU8wqXHxv6pCSEk9ZyJHuLMB7dShO19Xg3xkCY9Cv1aYBto02op6OV4wkbPCqwHbW+bYDx8Ijrb5Ol96XPhPGSv2Wea4HFmo6YuMAnd64KT64XsFdt7tQ+YKVnrPJVwygrLPJSIa2bma90TrNFySBbs3ct4plFInyWhyRtSO7ndTHakn1uHmjKCe20g93pq7EyRFkZtarlUk/0QiCaIos257Z7eugAzqiQKFO9sAZLOpM6uqmyXSlUtNIe/yfbtrX2xh2X5iQt6S5pysJiRr+IlegL3U3vRU24MGI6D0SqaGgQvgnqG8QKqoMQXfzmK6uAm6EXBIear1sV26VvOJNJFHCmMFliEW/Zs+UGS9Qwpk55AYnjJ1160sEPuAGJZ2MBE3jUeyug+aDkKtzy3XOz9nn3bYjtBTKGq5iAfEN60sTAz5+rWJLaVNl5XCvJVR6qbRiU2QTzSBAL3zvJXnFO8EmDJD7rhWQS34iAiJSSopDDPXyjQPNJdV05dTPG4RyyzCSJh7CRelQ8/1DDUxp5KS/FgZ704tSutiZVPoMh0SfNBqRYVayI3jjPb6uN/vn+dL4UkF6GP23hFhdycQto6oWFOK5ozX69UwGkbR2URh2k86SCj1+XzebjfIInP7Cr1GyO7o1P/FxFPj6OPg3fGchMIq3ZdYJphafaSB6CmSdCP4sg+8Djm9j55RV0M15eACS+QHLJYjLgjFEpBq9IFBuq/x5GivlptuNUqlaXVy2xW769nMXyNZz0+SBoAd01YYra8qkQdOL0HgChmfNa3r0ET69B3LDz1fpAtEuln9U9KXCxTW0MGduOglFHTAWrF3oe2INutJz5uPj1lMcbjBfuphyd/IbGoW3JKkVA3I6DNpyXPIOx9/ny/47I+Eq2XJN3oBggBQTdPeQ1GfEc3KAhJK/Gl6WgEFdVm6KkSzP3Hoej0xHlUlIFYstMj25nQpbdyI1uI2qsbNoshRfGxCsSoUwNMxQDnQwhnai/kSpsNPXTJaQ1GjNZHFrZzRhzzSPWsOCFBToLzSvpmqlL7y/+A7XO0r/Q7N8SlhQuEA1fcpnVbHo93NVZei40tzZkHbt3qrTI/Qn1ls0rCyEK7s1ckklP6VBoSepqoBmigoXB8mFbuEpEj1r5zba8CfAEzbTW4kNxCEUavdK6+EWfhMgzmojicIsJcGxh/rlQLdC0GqYhdZZP5ERKbePj4+aDZqSdMqxLdpqB7Xn2LmH9+fFtHLdKvx3RobWHYnStA4JtqpXi7lYxlINlHiMco2FcV+Ng2pkkv2KgCPwDvoPJWI5mabpflWFhWgmMDAGdzE8AngFExphhsdDZhToGzucWk2yNUbKlAxc4wDjpWyBB6m03IpeF69bWhxSKKHk8b6ye/aY+vrJ+Z96KtTf61jmL6vYVc0CHvJFURwvyBz9k8VFDQShNgifcuBtIjGe2dBAUzCJ5hHbnubGDjAnhrUZo4FEZt5GTnUn2IEaEAuayThlwy/alJTOmIWxliNgXZah0c5k3OyXpSxLyvzf4vAAkQg2RBwGwNWFpJhQJcmg1hsGIbX3d4EQDeRmgJujJVoBDiSlcS3OrqCm1lhJkcgEMgEtOGOgP86KVsIDpGJBeGOtfGgcI8a+AGyW4Gq6PRSmOpBTob4HDYjWPVYSrb03oki0tLbIWUe0Qi3xtLMtNzifAHd0XEsqPFCD3DV27Y7p/Dw69cvLQz/XZ/WEXCz1d3uVjCti62mi4ikkNEwJm+arjSSDqDq60QUW6UKpLlhqHYX+70VN+bHjx99tz+ZxEONfXrB8/qoyskbdks4UEcg+LruIEA55t9z2RCP6SFW09ynsnAdHweEhAsfPbwni8Z3ewhgw735EY8dd6UuMamlRcbXT/mUEynO9d3WcZz8+mCgK/0zIHFEOofoTcQbDo8CMIQDbr8iq1ob3nJn0CuL/319iPQWR6LxYqMxfI3n90X9DLSQRq7WwlqkNUrdGQ+HCInyhitNrJxnDgqbsE59BKwVSgFGi8POHZM8y3t6SUG4i+JzCxVXkT9372YPC1KK4JJiBDApIMmS6yJqTXqE6GbgQEfMp2inGfLkF4cFmsL+6I16iToj83XxgCskR01dKuTeyoh6KNTnBY71dp2KykVAOjikHDMWx0cht5dOajKxt6VnU0SaDrY5PqTNgw0qnJq7612E/0ncfQ38EgL66XScmji0HZX7QJImQ+F7vvo54bhVqr6qUWNBN2iG4Og70vvQkjJwo2nAaKMUMZrFLseSmyO3ZRV4bNup0qPVAmEUBaxbp4utbSU440kduoKEPoBkIm/TgNG0FCHjrol9t1B4RRIwnggjKRKSkG2kQjGkL+nxaOhX0JHRH7LpmhxAdUlj7QVu9UvRqLmnRncEegwkq54IEK8SM66ssYX3iYu23HTdpXfSV45Otg478bDf+0mMwARXlRXhbCfHpla2OGmbr4BW40UoLlT3yiNWm3wlLMPiDydFcPIgdk11bSa6OVVSVoabdxD80UVA0tkp1nLzNW/xSkU24d6mNpFAeqtlsI57OnbwGaSUbSLv0DN5R7RklEKXVfIYA+Az1bVGwoDQuRQ81YltwF59Dmh22F1qwzB3L8ecnRd/5AdOUwsjg5PXGKh4Db1t9cC+FTAMwNrT8gybIojQ2J+tBvvWpClmCsoTCsF+ZRUAi+E7INxQjlJOs2IWxvUGtScWU5GcyegWEz8mv9GEBi+kf4LdMLhVMVe2FA7kBjYrbq399bUNRUsc1YTeLqYgZ04TT1YhuxsUYFz5DrxxNFwGFG63NXQhorSDNRfQ2UVRnQ2DyEoKYrXoMJmJIRrGlnjJc4UGdgDV3F76Tc9XS7YBK2Eo7+o1JMcROAkNzgg+pr56uIrQZKOGjUHxj2NbPVrgF/tbL7oNPqDhjentcR5Gs0IfBOsQKStd8Q4iFoelBuqmmrQM+YgsaxfWX/tsMii7X5i/9XqcgA4vrHvWo0VIhkJtA469JBtfIQM0pfgLKEp5ou4UfKTwpFrKAuf0fTRwDQj6FpzFDldLLPAJD9rFJTGaAGYL+mIAipXsHQRatZvb6ld9yERrNpb42jl5nqouq3eOIqpF4Fh6khEprgSY8zUeM2YmJuuMXWMNxsaOtcrfyWdnZD+o5M4CUkWUFVrppXst5S6G9c/10VjbVwBaW6U01+//Xh9qIxmh5/QygIzuyxOYGL+j1fEpGhFFX4siDGKuip+J4G1wX8nCoH5H3y3d70p83VX5FqioNIqmInnrZlvncAke3QbZZOjJydxeKJf4yFGLrqqhOs/VfGxzq9FirN2d3Uz9b25dOPZ7iA0vtLgHmoX5CN+gMAA4LZQNafBGple4Qx844F/XZ/0KitDA/rqDeajGUmYt5woIPfaJno9qsjimgASva5v6bYcEERNPtVbmxP6m6rLoMUopiCmrrEwEBRgxzafGKpLucDonkN55H2PH4teCIEGJbfiPUqXJGCKNmPPKARAp6KyJiQmS9alXf/78+XPkTlpVgJwI8Pv3b6oJIt/gz89PCkqP/vr6UpBu/Pv7u5YMcGo1m7e3N1m8T+N98RSkr4PqW2uVbeTdeK09SRuuEAU02yESyOrCq/9oYqL06aenKlJyeYMmK1v7quPbKmAGixQXURhw7SlasPFXZUvon66cb4PhApJ4LZIN381DCbsKuWvSlijlVlQHstXVIzyRHY+lwipO0KvPCEhFCOfKJH6BqgnjcnaTESYETO1Z1kd7Esr9X0cvqXRqpDTAam/D9YoDgPoG1iTb9HI5k4ecdDTo+eBu6mc4ggIHNCul0HyEcuKq89Lsa9smqhPub7GANGjbqXjiUPZEdXGUchye7u0Zh+5WKjUsybyGPHtI6+4lO7LFCyRUrhRiJND8/en9aBIyJeXeN5EtcwjNtgek6Xq2PLAve66/da1Ca4P1GuviZQOQltiJi58COfcR4sxN2EXVxUw5RM1tzSwOSJSSVdaT46LwC2Rj9BMd9t9/giJIJ0lraHhIyfrpLblxVORBIgHQ8euyGiXsW0pRxhQUdMrTQoHpOTjRSs0CLJhi6Ann3wIuZnCESuZ8ug++Swx8bbojgg/x2XBGI45DV8IYxAEXEB2ZY1sFXpuoFVgcO6bkSnF3lU8C1gKG4fSn/NPKenUBume1MTQqnoFuSMOv/wihZYGXsCQsF38By+RvTZNcslvOzeY9Rb8xE3wfulolIiuxMi1Wbs2cZXRlNpvKmzgvytBdy1XhYQxdXEvKEj8rOrlp/Sb4qoK2eKYsBdRDE8QjZn6b55WVWwqhgWeJUnrP16QhklmufhS+qQtLfnOs/wtQ1t3kSHEGQRgWPSMfAGmET8QSxIYNOzZcjaMZjcURjKPqKb8u2bNATU9P/XyVX2ZkZGT2m+/fv3MhvKhuB3d/15FCf/Y8C2MNDH+H85mKGkKFP1e/RHFjUOu4s69FjtQnO+8+VtUNCaCPCqBFHwvENpPUVajksV2tF6X4rlNKUDUSRLVnYK27BZU3HfuQcUsa7B58DuMRRq0R77HnwG9r2oPqJLxUNtwzmozjSz0LEsg8UrxKAvFGgDeHuFPbGLhsRsgFwZ2gXfw45i8xsRySNfFjHiGSDk4ltFepRgazMjgJLlD6sUGePn36BJ3sDwbP7PuaiIfQuG2IfR9A/4ChnGCCBQcZkBvS23W8e/cO57l3JAVQIuTgomeMP378YEQgxF4roGEWfjt/agF0urdv37bRWMFe79rsHaRSpSglvh3WB8As0FQlZ5hWurvLVsIhU9h/d66Xlxc5zw749O3bN2wCXmoXvUvZr2nBOK3dNuhMqQTp2h07otyV9LeusT3/19dX4YXcTEK1S9xD278CETi+324d6cuiBeGrHW034z6heTtl5907jrPf7s0Zhds7kO0p9kBVSwqvTorTGe2AChQ8EQS/KwdFraPIJ7fwmYOTrzxdgwkkYZmgdqG2emBEupjJuVqytOhMg0mDXu6c5VJqqKmL1JCHgkGdYdUIvQMg85gLE3XaFFwU+6VHqerENiZJLpggXaMRnp3rYHqOwJcR383kj125bbhfSDo9FuzYHnUFdGsE+thxMwp1JkzoHgUvsyN62twtOWO0rIhLibAPOwiaRaVa1drGsX/3gX1+L9ytEIZspnrkenC7bcYsdEYkWO169ivu0qZmm/QOtGZQjYo6Enmn5joweI9q3eYdyC/qm0Q0wKtsTfiQHXnaqENsIMBjXxMiIPQlJnRkKJ26ueAi3dKhIPHbNkECCJcyGtFG3lo4S2qmDLoXpEGgELANjkBTPUhyzUQc6YLSRSpYHz3RvE/0sRKENrvGQMi7oKD9FqisssmGrQUJPyZb8hGhQR7Gl9cZRptru3HGHFPyZuAAK12fIF7HkXEBnpB4J8aJxTUSK6kxZMeUzYHe+i75HPW3P88fcgPg6wi7bl7tRxAFC+49kZh8+8WjsG3dhmKGredR7G4TedeeiU3swQivtZAzMaTPEXdOhLH/VvXbC3CWr9ha4GHkBYja3SeQieTgNIRUaTSx7H2WSjU2npR4Hamxe9x60TT4q4eqjnLt7geEAV6IU2F2O1RuLnYooal9MsM0GJw6bRu2vbaQuhF2BDAN50PuUAHBTd4LrvuwsxclqKIQC4iS+opLzdWm96Q9YPC9KjFUpRfHNIewO/qCsMmJjpiAaeZBdGAABw6BlyDWULmJHW6oTC04eTdPiWhJuME+JXsWcOny3U86mP3KlTWWJs1obkGIjDCAV7W+MwGE/2UOp0PYf+tC9k49pGgCB/QYrokZJ+u6h4o4OA5l76jpqtMhqWcXZIEITYGZtljStAOxapJOW1JwoOuppRtdxAfDt4o2wJ6/vVOD9B+qvRI9LetRt54TQ4Cn/CEwJSXkSWxMT0JZAOxCZ6LB8LZchFw+/jIFw1VS7dK3Clude1MxPI2aB3zt5Dq5mQCHZRMpF2qYdFbOSwkCdSOlBpq5M30wyCKFReCOXXhBJ4QBQLc4bw1WnGnCI9IHdyf+AMZqIGlU9esxK2GEg5MYOjspzCFZ+/r1qwi98w16SBKRD0EemT2uRU+cIRtOaTW9g8MedrdH2BeaIoDGSdVjKnjtLG4MIt+PZ0Z5puwsZaGX3gs4a9e8W0V88f17fwffZWxB35w/O5QDIvytvuAgaeEcuR2pO6OjC90pLtn7x48f+X/SaL8AFHjBCEPejR55l4Kc2AuZEELHJeq1kKBJOLDCbm/vD3+SKtho+4DHvj/fZ4hQ97fLy3bkfZjdKRtr27BkzqiO5dGmLxX+wBfwGEyTwMJQcmFwYWfZKXZGQpWhWSmHJAwSeFZObo6UjnMqEAGYKl01VcgTO/xKI0FspUKGWg3RBZmIDWXSET5cKWAPkEyE601vSJxTc2bdfQ2+EE+UV7gwG7MP1CfGhCte8Wt4roIAAZQNXsYm6IPQ4uyFgBPnxYQJQGhsDySldnO/oBWjGGaxhh3s+kRcsVKNxnIE3OUc3aHQI24mfqIngxvpiJBzdLOoJAJnzQFYF8UkGKUJKRaRT1Ch4+MF3FofXSrb5A0jjkTbR12tUoE4UM64tJlTxPyjVvTJGa4mBVUDrsvKmDOAo+lM9hGSCN4BBV16cj+Br5bI6lIVugR4OEPAta+F8pT8boeLsIjyB2CN9fEwZlrI87By3LRCuKL6A24SaxofYV25twgJJIxa7TU/6jz9PiYfUpaGXT0cTSBq9EA8MgflsJPqsapFToSC2nc0W0AtrcY8CaCtrR2AhWI1Gz9W7/9WQfnCJSEp8QJ7IU8q/shngDutIkocrOzBuVoUNVTALBpT/ZC9NFZMRoLl2L1pyFTudGiULuGrkWSgnD3oHmSXsHLaF7m+8Oo5xdXASjx9M78otnFY6Q75We6l0RNeW1zrknKMkljzi3vfBavbRN4duSQ7dH3c8137T4+kxgoNEAE2Bq35WnVrsND9F5+rEO8GJBwpH3hAz5xjAheAWIqmVgpGwS/LMcJoIXvC7Fr6+kNpCRC3I1TpbPqDRbTdAPfqdXyFW7u2Md00iwCga3Em07Gn2D/gxzsKWM3Fc3q3rUIrU7VGXB5GzY7DfKeCaewOUhufCTFy1bQ61HQeUtMKuPMyeUVx07UgOzzl1Rh6XvPugneTmevV2pGvoVhnwQGB/vv5c42d4VMStcLxXieskhBwT65VUq6sJBTUaeWdtFiAPuoCwrYlMSTNSMMi4SeUl1LJxljQvhS5kulHh+byJH0pCOx0i1VSBSGJM3LYogcPa6v9cf48WA29OidFeO3JAF9SWS9owDjyGonjc1qaraMnzLNo6UfLWFzmY0rV1oUvV32BSIirckw+X3+oi6lVGn3ofbQP50i0VuelNhKPE/fiRMq5qln/ThA5rZV34jee3r9/T1vjsgZq1ZKPgUTnDwIfuJfTihGsTG+fkE++k/hQVQa1tue8X2H+wEKbCCO44zfYFs7eO8oKqN59eEh9R2CVvLg94TKoxxRm1JaYrWqm/H/H9AF3CvdAm01/lOXYGbuRAXpTS0H5a+LFDEdJom5ni829gZdlCbCFnDMtG74UzThj2Vmb2oGr0gG92yMd49owc3SsxPdQpbsFLOp94rxZMb+Jw+hK5AbkAzJnOeNOpHwpsrtzhsn01O9tdn+SKELS42+fA2BqTcK/+G1jgkWJXaJlmnasZuG3OCwaU5tITHHb0qb69rcEFKWRYvy37AzlxM2VuPAj6g6obZeETrFV03OgmJGU1lqBp/mUNfnk9eRbnq79IQrJ1R/1e0BuKQaaq6BAD3BqH2luEXfrv54Y5ZGo53kyvT03tXsb3FyfML3lVqOXfge1ZVr4262UKvFWGYaQGNAixAXiMh2nAT2qXjav//KkhKsU0en0uC0Wp4UJJHgue2iN224AsXqJlFgDQ1SEdqNYVjkzBSFiK4GgwBp0sNYMEEulRK7fXPm3ecwMHOkoOnOCTcOUMPEhrhN1F01MVZtUop7ozC2RsTCFmcASS2ns2acvX75glJoGvv+yQDU49AuUoEmWm2ulfKz2jCYA7vP4E+VFiUvzwjTIAqv8tOI2cgZo8oLlsuj94Y5Js70PCG2ya4V4XpzPZWXzDG4BX4YibyadNzlWsNlWFcd3OnTxhTnYpBEUCj6ozmALPkAS27yluggaD5XgHzap+4Y3TVbOKyEtWBC3iFM0wgOeNoTCjTHn3GsjH/nBtCd69naFquVNIeQTgQmx2JaUNtVvYlPzlSBIwhMx8enz58/i7j5K9otFo9Xbdby+vh4zvd+8QWCq3xqn0zhOyIOPQx/LbACLvUbpQf8MSnF/P34l9eXjnIuZ7GN7zadwNDtpMdoA/hCPDU6gljpNednmYI+7tWEC/hc5sXM1NsAmkK5yKTsgb/tsWn3jNhtXiELiLGmmnLLpuWgsT4M0oaFA87jEZvoPoOd6d/Imlo90kD3CDZ62C0hUBSIxkHoxgQ815xqjbWRTQOoPF2Q1/+5o6Em35sY5YvlJ4yDTBkhjjjH/4q7CWa0navFMNERLOeSs6k62p10pGtoRidOwo3YHlwHB5ulljk0AYjKd19qxUIG7qfSIDe4VB8l8iHeTK2EE4EwgBjSxN72p9Il0saDynoAOp/yAVpohiN9BDXtiezJ+y160qVIwaKwJ2lW/2BXsmqig1dasEUdAAK4QiSY1roIXq6ESldz8i4YxS+AND7PoApyUyC6BVKWWbJBpBK2t+5ZMjd6f920HpAKMxlOkpnoEIoBghaOW2VjZviyEITSAszRVzgxhI4YE/l0i6qa5LIRUci4MsvdZgfTVciCya+3CT9jdNYqFdTwzgNNehjZsBWDKY75PYzNGh2Vh+vuiAs8PW2/vHztUWgR2U+fe5zsh7cUj0glezDp2GznvtHdZPi2oC2pF6JP0k6fmaHAXzoPDsiKVo+16wZRWi2exG9T4EIFNgWjoOrB9R54SFdCMJK0hRnFkvKfle7ar3SpW7974X2QRyGEri20hOKOGr3qA6kPiPc6/fgTuyW+lpkC/rgmls4RNteyqV0MSdfPAaGgPcri+c8QIz6pBWBP+/i7PDTO7I+USYdr4VNlu80ae70o4ANeubMJazeIlKCwOSSb8iXE16jdtlxeIDmNfKVxrrIIMRW5SKTwv24EYuBhOR12O1KvxVWksGtjcMDdFEHHzrpdi1yid+rtZJSegUqUmcsRZK2UhCzHaP2RzNfBCLrpsrWD73FIqSvtenITf91mwzW/ls8ocwYiyEKSd++m7j6QWYg5/B9Y3laX5dxA5rgq+YadgV1O+UaCR7pKbBkcA97qSmO3x2OLwrH1fdcGPgLwOIU25f6OHJfNIpTUUl8n75OEwOpfURI3uUDiHQjgBWnyGwANatb/++QGIrun3Z4bMppwrsX8dRn1lkxSPUKsp8qo7O/4+UK7ugeGdakZ/3Mf2NtwZnZR+SEO5B5s+kRzHcB1+BKyvw4uEXW8MzAVkI0LjztLZY2zimnkGhSkCwRx8T8iu5KpZHFUuoMN2+F9TMgRfdWwurwKPpiLB1+7jScVxXvKB+VSbqBbNyNUjIttEE228mnD6IiH+RSZRY7ltaIBtNwlkGEPAfFQ6eGW8Mz2P5klpAKzH9i1uOancvkFGOpEr39Zznw6YAdrjDG2XATHwd6B4YwDMi4E6n4W8YSKEr6ytr1Vo01UUaigcVSvv3oa1Ack1PQO5IdfmOlBR1iXdFio17WwluCSgPAjz96PsLusA5bxJXBgEYcJ9KYjxMul0q180DjDdgwblmhKOY8IE1ph0AD6SuLo++ZCOf/SIQCZVRulXDZdL70/EPssBlNW/jW9ze+hj5tkc/6riHl46Bk6qLzswnwj6a7Abxk6YA98SgDAZgLYR3eZkGvikUkkfOyOVJDjUNYNM+o6jqOMef9aMc/fmhj0NYZRH8AElPHp0Bk+JixugwKP3rybMSXMu7plADDr1qOS6fXURGprv43fJiUDF+pQta3ucJERSASU08szZd+VaXzlrcVC9py99ePrw4YMi+69fv8y0sh2axAgZk43sHn7+/EmTtXcIl6jFXl5e4q2xIiQV1f1NUReM1Lf3h3ApiV6jbXUJSN88bby4KOZPACIGhXGFWpSjHJALdyUAEOHNjm9lJdVEXlxkY/3r2zO6RAHs6pIpzxI42bm0QCLemFUBoq8jQtrxZXBzX9XFZutCaNrH/mUdiFCRHrhVrTGvISBmW1F72iDijBRaMi/FEXDsQaVWevWiZKXmBjDHkYbUdNdIJPNobvOau2y85tZiHy3p0wPpA3irWs3+3yFtTqHQE15HdzRA4z5X5f5lcFJFoIFSrvYtalpVIkVpE+3vzdxbCCph562fonaE6zv2/lFuco6ahHI4NrLtiVZytOrEB2pzidATONN3VUmAeG6sfsOVeEqgsS53wkmyFtZn9Nd/RpY29tKW9EmOTEFIZyuOsDmfsTRuQzcqE97nRU8YuBhn1mO0jNywGR7Kq7Wbe1p8BbLbQ8JhcMR/A292+r6Jm4e4AAAAAElFTkSuQmCC"))));

    #endregion

    public static TextureBrush Grain = new TextureBrush(GrainCode, WrapMode.Tile);

    public static Point[] Triangle(Point Location, Size Size)
    {
        return new Point[4]
        {
            Location, new Point(Location.X + Size.Width, Location.Y),
            new Point(Location.X + Size.Width/2, Location.Y + Size.Height), Location
        };
    }

    public static GraphicsPath Round(Rectangle R, int Curve)
    {
        GraphicsPath P;
        if (Curve <= 0)
        {
            P = new GraphicsPath();
            P.AddRectangle(R);
            return P;
        }

        int ARW = Curve * 2;
        P = new GraphicsPath();
        P.AddArc(new Rectangle(R.X, R.Y, ARW, ARW), -180, 90);
        P.AddArc(new Rectangle(R.Width - ARW + R.X, R.Y, ARW, ARW), -90, 90);
        P.AddArc(new Rectangle(R.Width - ARW + R.X, R.Height - ARW + R.Y, ARW, ARW), 0, 90);
        P.AddArc(new Rectangle(R.X, R.Height - ARW + R.Y, ARW, ARW), 90, 90);
        P.AddLine(new Point(R.X, R.Height - ARW + R.Y), new Point(R.X, Curve + R.Y));
        return P;
    }

    public static Size Measure(Control C, Font Font, string Text)
    {
        return Graphics.FromImage(new Bitmap(C.Width, C.Height)).MeasureString(Text, Font, C.Width).ToSize();
    }

    public static void Pixel(Graphics G, Color C, int X, int Y)
    {
        G.FillRectangle(new SolidBrush(C), X, Y, 1, 1);
    }

    public static void Gradient(Graphics G, Color C1, Color C2, int X, int Y, int Width, int Height, float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        G.FillRectangle(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Gradient(Graphics G, Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        G.FillRectangle(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static LinearGradientBrush Gradient(Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        return new LinearGradientBrush(R, C1, C2, Angle);
    }

    public static LinearGradientBrush Gradient(Color C1, Color C2, int X, int Y, int Width, int Height,
        float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        return new LinearGradientBrush(R, C1, C2, Angle);
    }

    public static void Radial(Graphics G, Color C1, Color C2, int X, int Y, int Width, int Height, float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        G.FillEllipse(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Radial(Graphics G, Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        G.FillEllipse(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Image(Graphics G, Image I, int X, int Y)
    {
        if (I == null) return;
        G.DrawImage(I, X, Y, I.Width, I.Height);
    }

    public static void Image(Graphics G, Image I, Point Location)
    {
        if (I == null) return;
        G.DrawImage(I, Location.X, Location.Y, I.Width, I.Height);
    }

    public static void Text(Graphics G, Brush B, int X, int Y, Font Font, string Text)
    {
        if (Text.Length == 0) return;
        G.DrawString(Text, Font, B, X, Y);
    }

    public static void Text(Graphics G, Brush B, Point P, Font Font, string Text)
    {
        if (Text.Length == 0) return;
        G.DrawString(Text, Font, B, P);
    }

    public static void Borders(Graphics G, Pen P, int X, int Y, int W, int H, int Offset = 0)
    {
        G.DrawRectangle(P, X + Offset, Y + Offset, (W - (Offset * 2)) - 1, (H - (Offset * 2) - 1));
    }

    public static void Borders(Graphics G, Pen P, Rectangle R, int Offset = 0)
    {
        G.DrawRectangle(P, R.X + Offset, R.Y + Offset, (R.Width - (Offset * 2)) - 1, (R.Height - (Offset * 2) - 1));
    }
}

public static class DrawTabItHo
{
    #region Grain Texture

    public static Bitmap GrainCode =
        (Bitmap)
            (System.Drawing.Image.FromStream(
                new MemoryStream(
                    Convert.FromBase64String(
                        "iVBORw0KGgoAAAANSUhEUgAAACUAAAAlCAIAAABK/LdUAAAACXBIWXMAAAsTAAALEwEAmpwYAAAKT2lDQ1BQaG90b3Nob3AgSUNDIHByb2ZpbGUAAHjanVNnVFPpFj333vRCS4iAlEtvUhUIIFJCi4AUkSYqIQkQSoghodkVUcERRUUEG8igiAOOjoCMFVEsDIoK2AfkIaKOg6OIisr74Xuja9a89+bN/rXXPues852zzwfACAyWSDNRNYAMqUIeEeCDx8TG4eQuQIEKJHAAEAizZCFz/SMBAPh+PDwrIsAHvgABeNMLCADATZvAMByH/w/qQplcAYCEAcB0kThLCIAUAEB6jkKmAEBGAYCdmCZTAKAEAGDLY2LjAFAtAGAnf+bTAICd+Jl7AQBblCEVAaCRACATZYhEAGg7AKzPVopFAFgwABRmS8Q5ANgtADBJV2ZIALC3AMDOEAuyAAgMADBRiIUpAAR7AGDIIyN4AISZABRG8lc88SuuEOcqAAB4mbI8uSQ5RYFbCC1xB1dXLh4ozkkXKxQ2YQJhmkAuwnmZGTKBNA/g88wAAKCRFRHgg/P9eM4Ors7ONo62Dl8t6r8G/yJiYuP+5c+rcEAAAOF0ftH+LC+zGoA7BoBt/qIl7gRoXgugdfeLZrIPQLUAoOnaV/Nw+H48PEWhkLnZ2eXk5NhKxEJbYcpXff5nwl/AV/1s+X48/Pf14L7iJIEyXYFHBPjgwsz0TKUcz5IJhGLc5o9H/LcL//wd0yLESWK5WCoU41EScY5EmozzMqUiiUKSKcUl0v9k4t8s+wM+3zUAsGo+AXuRLahdYwP2SycQWHTA4vcAAPK7b8HUKAgDgGiD4c93/+8//UegJQCAZkmScQAAXkQkLlTKsz/HCAAARKCBKrBBG/TBGCzABhzBBdzBC/xgNoRCJMTCQhBCCmSAHHJgKayCQiiGzbAdKmAv1EAdNMBRaIaTcA4uwlW4Dj1wD/phCJ7BKLyBCQRByAgTYSHaiAFiilgjjggXmYX4IcFIBBKLJCDJiBRRIkuRNUgxUopUIFVIHfI9cgI5h1xGupE7yAAygvyGvEcxlIGyUT3UDLVDuag3GoRGogvQZHQxmo8WoJvQcrQaPYw2oefQq2gP2o8+Q8cwwOgYBzPEbDAuxsNCsTgsCZNjy7EirAyrxhqwVqwDu4n1Y8+xdwQSgUXACTYEd0IgYR5BSFhMWE7YSKggHCQ0EdoJNwkDhFHCJyKTqEu0JroR+cQYYjIxh1hILCPWEo8TLxB7iEPENyQSiUMyJ7mQAkmxpFTSEtJG0m5SI+ksqZs0SBojk8naZGuyBzmULCAryIXkneTD5DPkG+Qh8lsKnWJAcaT4U+IoUspqShnlEOU05QZlmDJBVaOaUt2ooVQRNY9aQq2htlKvUYeoEzR1mjnNgxZJS6WtopXTGmgXaPdpr+h0uhHdlR5Ol9BX0svpR+iX6AP0dwwNhhWDx4hnKBmbGAcYZxl3GK+YTKYZ04sZx1QwNzHrmOeZD5lvVVgqtip8FZHKCpVKlSaVGyovVKmqpqreqgtV81XLVI+pXlN9rkZVM1PjqQnUlqtVqp1Q61MbU2epO6iHqmeob1Q/pH5Z/YkGWcNMw09DpFGgsV/jvMYgC2MZs3gsIWsNq4Z1gTXEJrHN2Xx2KruY/R27iz2qqaE5QzNKM1ezUvOUZj8H45hx+Jx0TgnnKKeX836K3hTvKeIpG6Y0TLkxZVxrqpaXllirSKtRq0frvTau7aedpr1Fu1n7gQ5Bx0onXCdHZ4/OBZ3nU9lT3acKpxZNPTr1ri6qa6UbobtEd79up+6Ynr5egJ5Mb6feeb3n+hx9L/1U/W36p/VHDFgGswwkBtsMzhg8xTVxbzwdL8fb8VFDXcNAQ6VhlWGX4YSRudE8o9VGjUYPjGnGXOMk423GbcajJgYmISZLTepN7ppSTbmmKaY7TDtMx83MzaLN1pk1mz0x1zLnm+eb15vft2BaeFostqi2uGVJsuRaplnutrxuhVo5WaVYVVpds0atna0l1rutu6cRp7lOk06rntZnw7Dxtsm2qbcZsOXYBtuutm22fWFnYhdnt8Wuw+6TvZN9un2N/T0HDYfZDqsdWh1+c7RyFDpWOt6azpzuP33F9JbpL2dYzxDP2DPjthPLKcRpnVOb00dnF2e5c4PziIuJS4LLLpc+Lpsbxt3IveRKdPVxXeF60vWdm7Obwu2o26/uNu5p7ofcn8w0nymeWTNz0MPIQ+BR5dE/C5+VMGvfrH5PQ0+BZ7XnIy9jL5FXrdewt6V3qvdh7xc+9j5yn+M+4zw33jLeWV/MN8C3yLfLT8Nvnl+F30N/I/9k/3r/0QCngCUBZwOJgUGBWwL7+Hp8Ib+OPzrbZfay2e1BjKC5QRVBj4KtguXBrSFoyOyQrSH355jOkc5pDoVQfujW0Adh5mGLw34MJ4WHhVeGP45wiFga0TGXNXfR3ENz30T6RJZE3ptnMU85ry1KNSo+qi5qPNo3ujS6P8YuZlnM1VidWElsSxw5LiquNm5svt/87fOH4p3iC+N7F5gvyF1weaHOwvSFpxapLhIsOpZATIhOOJTwQRAqqBaMJfITdyWOCnnCHcJnIi/RNtGI2ENcKh5O8kgqTXqS7JG8NXkkxTOlLOW5hCepkLxMDUzdmzqeFpp2IG0yPTq9MYOSkZBxQqohTZO2Z+pn5mZ2y6xlhbL+xW6Lty8elQfJa7OQrAVZLQq2QqboVFoo1yoHsmdlV2a/zYnKOZarnivN7cyzytuQN5zvn//tEsIS4ZK2pYZLVy0dWOa9rGo5sjxxedsK4xUFK4ZWBqw8uIq2Km3VT6vtV5eufr0mek1rgV7ByoLBtQFr6wtVCuWFfevc1+1dT1gvWd+1YfqGnRs+FYmKrhTbF5cVf9go3HjlG4dvyr+Z3JS0qavEuWTPZtJm6ebeLZ5bDpaql+aXDm4N2dq0Dd9WtO319kXbL5fNKNu7g7ZDuaO/PLi8ZafJzs07P1SkVPRU+lQ27tLdtWHX+G7R7ht7vPY07NXbW7z3/T7JvttVAVVN1WbVZftJ+7P3P66Jqun4lvttXa1ObXHtxwPSA/0HIw6217nU1R3SPVRSj9Yr60cOxx++/p3vdy0NNg1VjZzG4iNwRHnk6fcJ3/ceDTradox7rOEH0x92HWcdL2pCmvKaRptTmvtbYlu6T8w+0dbq3nr8R9sfD5w0PFl5SvNUyWna6YLTk2fyz4ydlZ19fi753GDborZ752PO32oPb++6EHTh0kX/i+c7vDvOXPK4dPKy2+UTV7hXmq86X23qdOo8/pPTT8e7nLuarrlca7nuer21e2b36RueN87d9L158Rb/1tWeOT3dvfN6b/fF9/XfFt1+cif9zsu72Xcn7q28T7xf9EDtQdlD3YfVP1v+3Njv3H9qwHeg89HcR/cGhYPP/pH1jw9DBY+Zj8uGDYbrnjg+OTniP3L96fynQ89kzyaeF/6i/suuFxYvfvjV69fO0ZjRoZfyl5O/bXyl/erA6xmv28bCxh6+yXgzMV70VvvtwXfcdx3vo98PT+R8IH8o/2j5sfVT0Kf7kxmTk/8EA5jz/GMzLdsAAAAgY0hSTQAAeiUAAICDAAD5/wAAgOkAAHUwAADqYAAAOpgAABdvkl/FRgAABJlJREFUeNq8V0uOJTcMoz65QJL15AABgtz/XjObp08WLLOcTtZTaBT8yrIsUxTltj//+hs/8clvf3zjaBfutrtuAaC6AJiZGcwMgMEXM7Nmrz0/8g1gd7987+nw4KzB8/v3H7TLzN3t7og4EayZ8d3d7r677j4zHM8MgIigAY25NiK4hN+1yjnHwcxkJgecPkc0OuUyBsTv3FhwzS7/PlU0zszZVYhp7tX9HAd4xoCZ9YyZyfQ59PHLL2bmZvxO2HmyiFggIoYgAdUdEW6w8MAuFjvj5th1cw6mG7tdtTPYpXF3TzdtDNbdVRUe021AV4XHzhhsF1jQYXhg4QTA3Wc6Imba3c2wO4zX3SOCWQS2u5hFYM1Q9TlpfmCICPKmu2hP57vTXSkK0ClBqyrlDAAzL3aICGIZYefPqqKZaPjAu5uZzqC4oGfcnTtFRHXfhHJ3pm0BRrlP0rG7PTO7ADITrB8zAJ8q0aK6cxe7mGkuq5mINLOZdXMG6x7dZWYZCcCA7gaQkd3F2cxfqj6fzycisUufZm6A0XOVmSUdqR4iYmbMHNiZ4RS975N90Jg/mcjM3B1CR30gD2jPRPDEKUCI5AEMb1VcedKj+PhmCphCvumEYwlId7uKRntIO2a3uunrU8UCZ54oCPQreHpmAZFAQkMnM7PAez5lmC7MLMwkUczBzNjBk/spyt2N813ixzNhT2qAfELrxuVIe1Nc9OaByG8aCyjZs2DkUwd9pEox3hLMsZRaRyFQmtV3wUByMRRKsT4yplT93Q2BJxA/RTCFwjhEKKb/ZgfXcsubMllVDI0Q3Vuqm8jFfyWGxlVFBWFwOi6dy0N3p9KjoO4GJi5wJ0XzBJtJG4UvJFRdpwk/EPr/VpvS8NTA4Rt/0inbAtVcoNl5VIva+0FYjFIr15sW1KE7UukLkVRSpey4dJUbMwgAfh/uCzOVVxWGbg/qU3ZqdI9YCySeXrPihAt6wSi4lQzVH8tZ+MhS9dfdmamw1NcIZAq3L/pyl7B2FekJIxfqznE3PLm61+5uamJ5e9DGu3Y6gyrkruvppj2ZosKfmZ2xUx5vItnCbq3D2bu7zb9KjzZ+dCBCRP2i+Dgn1mUQgJ3r5KtAtyT6eW76iGkk7V0kzw3hiJ/K192rSh002TNnxp1B8YjYZeyvUke4sI9wUt3MInj61U2cs4dxXEjYPWcE1PNedtFdjzBguiOzq6bnPajFHh6xH75EdZ8emGHXIzhr7kNspQUvFc242XOvBaoKZgt4xDxeZumad+eIyDR3Iw+pKafTcfB4EKdvmSenVe+6f+reoIqUmM15uOQeqy8CSMLdTTmeblbxZEZVn3JEZoiKzNbMuj9lqoLjRWJXyD9NOsKBdbfE7s66O3a7muZdzxUIp+wAeATYcXYBww7WnEQ4UjB92Mfy2HUzmE0f0eANR5cyc2ee9hQid5IGzlFCpplZedu8O8z8dLv3BsXyJ1PfXnVx50HSnSKAc8Fipd83MOYSV3xqs3fj9IjuTt2v3haVcbdDdXMzzPyr8R7VfVr/kRj9Q+K7dvemCLdff/v9Z/7//s8Au8Ajdwrx6VEAAAAASUVORK5CYII="))));

    #endregion

    public static TextureBrush Grain = new TextureBrush(GrainCode, WrapMode.Tile);

    public static Point[] Triangle(Point Location, Size Size)
    {
        return new Point[4]
        {
            Location, new Point(Location.X + Size.Width, Location.Y),
            new Point(Location.X + Size.Width/2, Location.Y + Size.Height), Location
        };
    }

    public static GraphicsPath Round(Rectangle R, int Curve)
    {
        GraphicsPath P;
        if (Curve <= 0)
        {
            P = new GraphicsPath();
            P.AddRectangle(R);
            return P;
        }

        int ARW = Curve * 2;
        P = new GraphicsPath();
        P.AddArc(new Rectangle(R.X, R.Y, ARW, ARW), -180, 90);
        P.AddArc(new Rectangle(R.Width - ARW + R.X, R.Y, ARW, ARW), -90, 90);
        P.AddArc(new Rectangle(R.Width - ARW + R.X, R.Height - ARW + R.Y, ARW, ARW), 0, 90);
        P.AddArc(new Rectangle(R.X, R.Height - ARW + R.Y, ARW, ARW), 90, 90);
        P.AddLine(new Point(R.X, R.Height - ARW + R.Y), new Point(R.X, Curve + R.Y));
        return P;
    }

    public static Size Measure(Control C, Font Font, string Text)
    {
        return Graphics.FromImage(new Bitmap(C.Width, C.Height)).MeasureString(Text, Font, C.Width).ToSize();
    }

    public static void Pixel(Graphics G, Color C, int X, int Y)
    {
        G.FillRectangle(new SolidBrush(C), X, Y, 1, 1);
    }

    public static void Gradient(Graphics G, Color C1, Color C2, int X, int Y, int Width, int Height, float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        G.FillRectangle(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Gradient(Graphics G, Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        G.FillRectangle(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static LinearGradientBrush Gradient(Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        return new LinearGradientBrush(R, C1, C2, Angle);
    }

    public static LinearGradientBrush Gradient(Color C1, Color C2, int X, int Y, int Width, int Height,
        float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        return new LinearGradientBrush(R, C1, C2, Angle);
    }

    public static void Radial(Graphics G, Color C1, Color C2, int X, int Y, int Width, int Height, float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        G.FillEllipse(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Radial(Graphics G, Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        G.FillEllipse(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Image(Graphics G, Image I, int X, int Y)
    {
        if (I == null) return;
        G.DrawImage(I, X, Y, I.Width, I.Height);
    }

    public static void Image(Graphics G, Image I, Point Location)
    {
        if (I == null) return;
        G.DrawImage(I, Location.X, Location.Y, I.Width, I.Height);
    }

    public static void Text(Graphics G, Brush B, int X, int Y, Font Font, string Text)
    {
        if (Text.Length == 0) return;
        G.DrawString(Text, Font, B, X, Y);
    }

    public static void Text(Graphics G, Brush B, Point P, Font Font, string Text)
    {
        if (Text.Length == 0) return;
        G.DrawString(Text, Font, B, P);
    }

    public static void Borders(Graphics G, Pen P, int X, int Y, int W, int H, int Offset = 0)
    {
        G.DrawRectangle(P, X + Offset, Y + Offset, (W - (Offset * 2)) - 1, (H - (Offset * 2) - 1));
    }

    public static void Borders(Graphics G, Pen P, Rectangle R, int Offset = 0)
    {
        G.DrawRectangle(P, R.X + Offset, R.Y + Offset, (R.Width - (Offset * 2)) - 1, (R.Height - (Offset * 2) - 1));
    }
}

public static class DrawTabItAct
{
    #region Grain Texture

    public static Bitmap GrainCode =
        (Bitmap)
            (System.Drawing.Image.FromStream(
                new MemoryStream(
                    Convert.FromBase64String(
                        "iVBORw0KGgoAAAANSUhEUgAAACUAAAAlCAIAAAE9+4fCAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAABoJJREFUeNpMijEOgDAMxHJHykCfgoT4/7eytTRHy4Q3y8Z53fbDj1r3Up5MSU66xmjSLFJ20Fv/FEZgPh4RXLKtwfIVQIzo5vHz8/8FKmMGyQNNZfn9+zdQ4Z9fICMZGRlZfvz4ATEMyv/6BWgekAfiA80DCED3GBwBDIJAUAEbiCkmj0z67yiMIObUd3gwzHHAkq/7kVK6O4u42a4XzoCNAniqI0L1hbYzHNBRiHmLmBzMpDbn0MBvTOTdpWlbZxN48lxHfaSAhRgA+ahn+o9PALrIIIdhEAaCtuEBVf7/xeQYVWDsDtBDW6kcENiw2h3/Rv2K/TgOXOiKtdfbsBWbCESwoZOjQLFgT7V7N8X98ALbCHc3kd7aGIN6de8o9GiIoOaLw6QBtu7DprYhskniPyU1tc7BpLQVnzpfeccOh5qRPpir8SIyaHOlh1Z93rfuIcjHQNahXtf5L/tLAM7JxMZhGAaCIq2uDgiup7SVblJNHD03u3RSwAmwIK8+cpaKn9svB0KRXRXqsglKnzowCn0AIQ5Pld4ZxppsGu8hC6LtDztsoUkpaU5CjwwokXhnGn0tEeJIJsMQaOVDnSAiLcAN0joYqmOu2Y9On5H0V6iRNdC9JnPk8Tpf9KI0RqeCiAe7tslU2WIr4PE0L++3b2UJoKtwyXOxOi0rNR5I8+ot/1UnX5jTNlXamaGd08EgOWWpfqxRv9+GLsdWu+4Uhgw/i61ya+08dVCBQAADutHKY35drxGP5739q/0JwFm57UYMwkCUEPr/n7tSA6Zzxk7al26l5mFFWIJhbn7nkDcPmXG7pbdvylMzW0GAaETMWrkgSRZ10VENA/G7TYZ+IUPf2HKa0HguIMdfYwxtnFYaH6fLoDIHzmnpgOSaEvMuMQmqz6uHbcU3IhEXYbIgBdPXaNd4wqZUZ5v3cfgysaBMspLuJAt0h12ODAzk0UuJei4JU6vF+2UFaTWG2mVmM7YSm/z38J1JZ94bUs4iKpilskjgFAtk3aIxmmVKKZsQ2Tt/7eOVr7rmYVJyRiu129AJW6qmRTHWduVvxwW9dWw7w3lSdhsP8cFO8VPBtg67cqsDAmFrT5BsufrJWIv1UUliEzb9Pc+LmsgURs0r4IQeErXarg3DeJJgEQWpDopXKbaivtx5Pbu4JoMtcuwkjXG44+xJxG+gisM05Hm58xTXRJb7Drmrww221u2X8VNMq2lYHz5MgSQ9Pm1lYQU109frFgHhf0FoxYwGBXLmuYuvvdDq/wz+R3v87fkSoPcyPaocBoKwLiKjaovcyIkYNgBi2J+w9eyR6GPk5wgAatdW6RzNfN2ur3/eNhqB4lYMv5WiIDFo9wG+nsogITwgICvQeetrU8i6240JIdOOooyv728lU3N1+Z55HcbK7dUy5SzIDus/X1u7FvMD4+1D7IK1pjR21UGmBEpSL0E6z+VMwT1qTdKrsvNo8k4obe9AY7NyHBOqBe/JVb8bUz6GFmeVL6llnAdelZ1RrNpM1kO7plb6X1PDD67jJ2B1FONXm0lFdAAGmaEEpoSx93DyKsidebkSbp23ZW6gKk68at7m9dDZirwUmtFeCGSqK1fFMxv7yyhCqfaykkoblBHpaQw2WBer87SUhw2YjYMoLDU3l07GqV97HV3aClPouTzGfmFtAUo86/QgFxYL8c8U9+zpsGq33F+JQzZEAjqRXWDE9EsQ48hwEoml6ZvIFhH8QjH6w+S5URlUzHdLDv7yWQcnnMmxaUeIgcOTpkzox8pgT3ZA+JQUNo8Ymq+kbaSATNFQ9zZulbOXZKPlhbHJemzLc2FgkIQkbJogB0cGx8706unFeFD6kyCUKa9104IdVF9eTPWuGDLYPQdnuuiqn2a8bBSIhitrEudrZXtARDJGy7oyFtbK9MGywdPCzGkz3CBlm9Snl+id+4XsSX4uIDGqdG74L3OylNqehm96kv2JEgxveWYfeg7vfabiSD10yRomR4dvgd4kHMtIdD5tVW3r1q4wVHkgdnrw26cZzsghpM6QScj8kX6ZCxIPbbwLx8rmGr4ePPeawGOdtLLVlSZtCjuS8+tGE0S4PwdQ3yBsD6XdLZKkYyH1EeFlq8TGky3bOWVCLol4HTWmZab6O4k5eC5bk2GJ5ZRBSfXn0XnwQGVlWocttlxc2Zl2HA9+cB3n9bWVbugRfXN1eZZp3eDw+vH5Xn7xZ3z++/ub6/0AKOOX8onK2egAAAAASUVORK5CYII="))));

    #endregion

    public static TextureBrush Grain = new TextureBrush(GrainCode, WrapMode.Tile);

    public static Point[] Triangle(Point Location, Size Size)
    {
        return new Point[4]
        {
            Location, new Point(Location.X + Size.Width, Location.Y),
            new Point(Location.X + Size.Width/2, Location.Y + Size.Height), Location
        };
    }

    public static GraphicsPath Round(Rectangle R, int Curve)
    {
        GraphicsPath P;
        if (Curve <= 0)
        {
            P = new GraphicsPath();
            P.AddRectangle(R);
            return P;
        }

        int ARW = Curve * 2;
        P = new GraphicsPath();
        P.AddArc(new Rectangle(R.X, R.Y, ARW, ARW), -180, 90);
        P.AddArc(new Rectangle(R.Width - ARW + R.X, R.Y, ARW, ARW), -90, 90);
        P.AddArc(new Rectangle(R.Width - ARW + R.X, R.Height - ARW + R.Y, ARW, ARW), 0, 90);
        P.AddArc(new Rectangle(R.X, R.Height - ARW + R.Y, ARW, ARW), 90, 90);
        P.AddLine(new Point(R.X, R.Height - ARW + R.Y), new Point(R.X, Curve + R.Y));
        return P;
    }

    public static Size Measure(Control C, Font Font, string Text)
    {
        return Graphics.FromImage(new Bitmap(C.Width, C.Height)).MeasureString(Text, Font, C.Width).ToSize();
    }

    public static void Pixel(Graphics G, Color C, int X, int Y)
    {
        G.FillRectangle(new SolidBrush(C), X, Y, 1, 1);
    }

    public static void Gradient(Graphics G, Color C1, Color C2, int X, int Y, int Width, int Height, float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        G.FillRectangle(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Gradient(Graphics G, Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        G.FillRectangle(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static LinearGradientBrush Gradient(Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        return new LinearGradientBrush(R, C1, C2, Angle);
    }

    public static LinearGradientBrush Gradient(Color C1, Color C2, int X, int Y, int Width, int Height,
        float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        return new LinearGradientBrush(R, C1, C2, Angle);
    }

    public static void Radial(Graphics G, Color C1, Color C2, int X, int Y, int Width, int Height, float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        G.FillEllipse(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Radial(Graphics G, Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        G.FillEllipse(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Image(Graphics G, Image I, int X, int Y)
    {
        if (I == null) return;
        G.DrawImage(I, X, Y, I.Width, I.Height);
    }

    public static void Image(Graphics G, Image I, Point Location)
    {
        if (I == null) return;
        G.DrawImage(I, Location.X, Location.Y, I.Width, I.Height);
    }

    public static void Text(Graphics G, Brush B, int X, int Y, Font Font, string Text)
    {
        if (Text.Length == 0) return;
        G.DrawString(Text, Font, B, X, Y);
    }

    public static void Text(Graphics G, Brush B, Point P, Font Font, string Text)
    {
        if (Text.Length == 0) return;
        G.DrawString(Text, Font, B, P);
    }

    public static void Borders(Graphics G, Pen P, int X, int Y, int W, int H, int Offset = 0)
    {
        G.DrawRectangle(P, X + Offset, Y + Offset, (W - (Offset * 2)) - 1, (H - (Offset * 2) - 1));
    }

    public static void Borders(Graphics G, Pen P, Rectangle R, int Offset = 0)
    {
        G.DrawRectangle(P, R.X + Offset, R.Y + Offset, (R.Width - (Offset * 2)) - 1, (R.Height - (Offset * 2) - 1));
    }
}


public static class DrawTabItNo
{
    #region Grain Texture

    public static Bitmap GrainCode =
        (Bitmap)
            (System.Drawing.Image.FromStream(
                new MemoryStream(
                    Convert.FromBase64String(
                        "iVBORw0KGgoAAAANSUhEUgAAACUAAAAlCAIAAAE9+4fCAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAABpVJREFUeNpMi0ESgDAMAgNJ9THOOP7/bS0penIPwB7AdT/xo2oMAEnMJY86PlfrzHIStHQEJNGs+ZafzJR6C8DGHKQAAIMwEKyNYP//XqONPfe2sDD291Qiu9tPRDKHeaqTBKBfLDj22MtGBdRXADrHGAlAIISBAW7OXn2HheP/fyYxHLUpKIBsYtf9NJ9LFeSu7KgzxzanCFp1SH+gqiQNDoOgC4k3U7PcpIccVT9AfY+aSiGabOYqbvtx4l+fAHyRQRLAIAgDVej/X4w0G3tpD2U8CDqQDV/UFzZMUsQktEidpB3lqdKaehUryLU7kA9kLmNMwnwY0tX7SofKtctuxpkUg3+rAGYxxwelPbo8OOcTNGZz2DRsfy8SnCJa7BrhHyifgXP0LW9DRR/d/6y5BeCZXHAYBmEYGgK9zzTt/hebgD070apW4pOG4OeM1/vjBBhjGtrxlbKOrWm6fk7UViaDE2ddoUOHtB+IllT+S2XAaWNKDMjlBG9se+buIZ51GljRileVhqZCHeN5Hu9Kh4phyqJoaBE0AQelwP1TcMzrSkJiWNsqKbxZrLy93BfREiIw5fLdIjTMssa9RV5GMrUmulg3kE2s0EagXEYatk90IdUhVC5N/eAgC6yg7XQ8XlKmtaY0lmhuEHdG1HR2a4W9NssklIGwta57SpvvN+kSc9M5wpBxroufdOCffoNV6ecnAF9ltOUgCMRQBvH/f1hhk5tp37Y+9AhSGTO54RchP651i5mxp/GVSd+yQ654xfPlOvfuPQudTKy8MzautqBG7f3wN+dth1nEGbVHLGocLRI85uZEA8yux/KXBJPpBAHWM7ziBA9qtwaZ5pFwlnvQEvu421AN5rK7y46Um9RrZ8MEemGuGXynN94XFQ2hnxdVPcdLBYFuCEc/0L3cbp1w1uMv2hl6qeq+60qiXPaEG6M3r48hb7pap6IKFnc6XjH3gaKYM1eyp43Cmu+MBw9hTAIuu66aoCQvnM3EUlLKKw9Kvpw3GyVTsIqxS69hzCkpmRYyJ0kBFC39CFBaqR5kK+3/lb4pCxQG5OTDurN2LK6nE7vNlQyfc/G+niSL+sTJyeemsV3Caq35ksgrAefMY7vvMG5WpY621wE2OpYp8lg0aqHWnG3hP0N72s5+AXpn/cqAYD0mhYozY93BVL/u60Thx/+fSeIDLBRQ0TX3XRLgF7JJX3/G7+Pxv+tPgK6rBMdtGAaaEtMfFSj6/1clOjoHpSyaBYJ4Iys0OZfi95+/BA6A0guEpvAlpRuPIbiI4jg1Nv779jFhRUJTWF5blBTbI9/vt3d5xuepO84uIleEVFPbrb2vg3sLGVSUd0kJMNpW7lkIqP3VdxgoBB6wn/ZaythDGbOeQfBlQOx3JzUn31WTGe0qLNW9pQZPcvcwHPRiuR174t7kfIjzlGlOd7JBFGpp7dwkNsw6ftABMEQ/0N0ah1HZ87XG8HSqW3x0wKkcIF0pC8zXDneScmulxmW+flFx8iVEzibh8iVVqThZTcOCrnLJCsn6VnyxokBoJ/hsecBXcG5FHfmDgC6YuyD6cLZ0xhApaBQsRQ/aTmowRrTD90VOwUYgiWPNPAqr5u1e4tHcTeo0726TkZipagX/sQyg2BKeSqTYamr8NjWvceLDh9zl9nzNOaxRZ8xLLdVei39RIYzzp+sYBS2tBEexHn/rPvfISsab2bapYx1CzSTJ2ovq2+Zdysz0JQNpzzExz6OdILTPsKcFU22wPA8pDdbjg2LSoni5pddVlKg4icUWpXwPOKIiixiivX9XDWIbhY48uuG4to75HsmP6x+rwoBUSz/TBXrHZZZJE1EF9o59hqEA5Yvw2aFJBnmgoFaHQ55gSKAzlbXuwFKmhEtwwweBphjbaBqQYYqII0ct1Y0SLa5Z6ix4qyMYN9numFGrDGcQOL/vSnXMdRoeqM1mllw4wXiZ0M9vJ+c/qXwD7FTcWYwuTkkypeVnLOfDc4whdejt5IG41nIPHheERHa2C0n0SK4ZTryGt85+LY5BezfGBjw4Tj6ZfV9XoPPvyyer0tCjXER8xjQKFm/v141NX4zQ79jZ+DRY8tjiuhmBdFgKhMEa71Mq5nz1QvGNtIsfCmIFpw0dO7Nd+OyHEJbSpGWIuBCkONU5j+/sH+fduIU/5wTJWUCvz7nurh9juN3Ac9i9xiSFmxbd3PWccgD8ub4x4Z5DvMAp3xHXs+At6NNxOwyuv8TFz1ym7/P8Aycx63ToGYMiAAAAAElFTkSuQmCC"))));

    #endregion

    public static TextureBrush Grain = new TextureBrush(GrainCode, WrapMode.Tile);

    public static Point[] Triangle(Point Location, Size Size)
    {
        return new Point[4]
        {
            Location, new Point(Location.X + Size.Width, Location.Y),
            new Point(Location.X + Size.Width/2, Location.Y + Size.Height), Location
        };
    }

    public static GraphicsPath Round(Rectangle R, int Curve)
    {
        GraphicsPath P;
        if (Curve <= 0)
        {
            P = new GraphicsPath();
            P.AddRectangle(R);
            return P;
        }

        int ARW = Curve * 2;
        P = new GraphicsPath();
        P.AddArc(new Rectangle(R.X, R.Y, ARW, ARW), -180, 90);
        P.AddArc(new Rectangle(R.Width - ARW + R.X, R.Y, ARW, ARW), -90, 90);
        P.AddArc(new Rectangle(R.Width - ARW + R.X, R.Height - ARW + R.Y, ARW, ARW), 0, 90);
        P.AddArc(new Rectangle(R.X, R.Height - ARW + R.Y, ARW, ARW), 90, 90);
        P.AddLine(new Point(R.X, R.Height - ARW + R.Y), new Point(R.X, Curve + R.Y));
        return P;
    }

    public static Size Measure(Control C, Font Font, string Text)
    {
        return Graphics.FromImage(new Bitmap(C.Width, C.Height)).MeasureString(Text, Font, C.Width).ToSize();
    }

    public static void Pixel(Graphics G, Color C, int X, int Y)
    {
        G.FillRectangle(new SolidBrush(C), X, Y, 1, 1);
    }

    public static void Gradient(Graphics G, Color C1, Color C2, int X, int Y, int Width, int Height, float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        G.FillRectangle(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Gradient(Graphics G, Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        G.FillRectangle(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static LinearGradientBrush Gradient(Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        return new LinearGradientBrush(R, C1, C2, Angle);
    }

    public static LinearGradientBrush Gradient(Color C1, Color C2, int X, int Y, int Width, int Height,
        float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        return new LinearGradientBrush(R, C1, C2, Angle);
    }

    public static void Radial(Graphics G, Color C1, Color C2, int X, int Y, int Width, int Height, float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        G.FillEllipse(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Radial(Graphics G, Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        G.FillEllipse(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Image(Graphics G, Image I, int X, int Y)
    {
        if (I == null) return;
        G.DrawImage(I, X, Y, I.Width, I.Height);
    }

    public static void Image(Graphics G, Image I, Point Location)
    {
        if (I == null) return;
        G.DrawImage(I, Location.X, Location.Y, I.Width, I.Height);
    }

    public static void Text(Graphics G, Brush B, int X, int Y, Font Font, string Text)
    {
        if (Text.Length == 0) return;
        G.DrawString(Text, Font, B, X, Y);
    }

    public static void Text(Graphics G, Brush B, Point P, Font Font, string Text)
    {
        if (Text.Length == 0) return;
        G.DrawString(Text, Font, B, P);
    }

    public static void Borders(Graphics G, Pen P, int X, int Y, int W, int H, int Offset = 0)
    {
        G.DrawRectangle(P, X + Offset, Y + Offset, (W - (Offset * 2)) - 1, (H - (Offset * 2) - 1));
    }

    public static void Borders(Graphics G, Pen P, Rectangle R, int Offset = 0)
    {
        G.DrawRectangle(P, R.X + Offset, R.Y + Offset, (R.Width - (Offset * 2)) - 1, (R.Height - (Offset * 2) - 1));
    }
}


public static class DrawTabPageBack
{
    #region Grain Texture

    public static Bitmap GrainCode =
        (Bitmap)
            (System.Drawing.Image.FromStream(
                new MemoryStream(
                    Convert.FromBase64String(
                        "iVBORw0KGgoAAAANSUhEUgAAACUAAAAlCAIAAAE9+4fCAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAABKdJREFUeNokikEKADEIAzUW///RpfSs1g01EJgM0W8fAKp6X8gQkaoahYjo7hlmBjYzeRm1SO4+F+YXgMU5ugEABGEoGAnsP6oxLuAR5K/0tbD2uQSHkZlB4KWstMZQWpmQ9k1V/fvNRIjMM08AnscgBQAQhmHo/18q4g8EUzPmoQfXZe1Y++DHjGJkpYMCDYJdlhzgUytdt0mUWXDDAcuXx7mHyieb9WK3mKXvf7SV+gQgig5yIIZBGIq2Ve5/0VHVfVbz0xepLCICGLA5f/ejm9VwyF+09oAxgubPOcsVqfklQUU09rwMmUKG5VSds8iQzrV66/lBlR+v0bso3nsFFZYMXXyliekW4ZwO48HThzqg1V0GULI/1uwvANN0kAMgDAJRNJLe/6yN8QBCnhnbVbUUBv702vej/dMIiEM/Pqqy6RhghnIGjqaip+OUMj1JsVhkAejABVz0QFSqKbBCirbGS6p5CaIrlD6vEYNr7OenOCogjro50nfiTmYu2PAL/TiutGElVC7TtjyFqvonZJgqewpy+QxS+17tsJEjDR+mB7ON+d3JzO1fAbisox2EYRiGohLi//8YiXQnu0zsYdpK1ySO7XAU8nu5upc0LWrsLE4oOJ9g6FhhRCYySNRW8LUa752tq8vJa99c1KuczcjXkslCpPok03xZzYfARe8Bng4Kj4S5okoWUYyF1TppO4ttHpZzhKJPMs82+PJzX3I5d10OBnADM0udhxx+xckR14Hv/kKc5Cq4o9dMkB2JFBCz4J4FV+GrYWLTBG8FHnhIGRNfhe9MmZ1QkE1S/Utk54azc/EkOwdXcO3SrSO/WBeMfoZe3sha4teOu9jM0RDPou/1Mw7sTJC93VYStfiA+U2d5LQcvYpRXqh63tFgCPgN5ZsG7tpdbU9r2r8GqcvuAjZ6pzYCN2S/AnRdRzsRwzAQRbUV//+pCPHMw7qc9GJRCamiG8exZzyT1+fXN0Cph9InpFFOewNNlP839eQ70WcbitjEOdq2JxIptl7xHXzysOXewAvMpTqZF+nmPKz98KPcgPY5LlB0yj3ohahdbV+jsNEeaTLfdWl0O4kbZRFSFHVukMd3CzvxVAhIslJbjg65EgU5zjtKbYHSmPL9w+nT1HlIbl+x0PaHpg10yQbNCW0Pi0Ndlg3ZJNRUpO8ZDyTYpuP4RMffIGyEWgYXUtnaiExp3raclTGUXakykVG3WYPxIlaorIeyb9+cdkSkKtGQMjoPHMhZAAnE+YpOEKCUOilVCV8bXmZCLbvz2G3XDOfOdbA2GzsC6auI2fRoU075FpU/Nwro8KHJtv1Eh9vm9DaEj0NR55/fR3cbxknRHY03MMFlpFBJoYmzw2GuvjbbM7zJlalkrVXnfPUc+ZCsa1PA2dO6hJwAvWphcrZbLrnjTrfZ9YvkINqWlg02AtXqGLXlEZJPKd6r5q+Z1BxBkrTveh7AAY3tpxONrRI5A9HOXSgsWTBVVhZqApMy0KT52iAty6xQU5cMdPV8Pc9l/WwD3NoDBfvmEZqqpPRFRB57d1/P0skyL3JlFbr+FV0GETHJD7ExdeuAAath+5/z8gaUYeDAmgwDxAAAAABJRU5ErkJggg=="))));

    #endregion

    public static TextureBrush Grain = new TextureBrush(GrainCode, WrapMode.Tile);

    public static Point[] Triangle(Point Location, Size Size)
    {
        return new Point[4]
        {
            Location, new Point(Location.X + Size.Width, Location.Y),
            new Point(Location.X + Size.Width/2, Location.Y + Size.Height), Location
        };
    }

    public static GraphicsPath Round(Rectangle R, int Curve)
    {
        GraphicsPath P;
        if (Curve <= 0)
        {
            P = new GraphicsPath();
            P.AddRectangle(R);
            return P;
        }

        int ARW = Curve * 2;
        P = new GraphicsPath();
        P.AddArc(new Rectangle(R.X, R.Y, ARW, ARW), -180, 90);
        P.AddArc(new Rectangle(R.Width - ARW + R.X, R.Y, ARW, ARW), -90, 90);
        P.AddArc(new Rectangle(R.Width - ARW + R.X, R.Height - ARW + R.Y, ARW, ARW), 0, 90);
        P.AddArc(new Rectangle(R.X, R.Height - ARW + R.Y, ARW, ARW), 90, 90);
        P.AddLine(new Point(R.X, R.Height - ARW + R.Y), new Point(R.X, Curve + R.Y));
        return P;
    }

    public static Size Measure(Control C, Font Font, string Text)
    {
        return Graphics.FromImage(new Bitmap(C.Width, C.Height)).MeasureString(Text, Font, C.Width).ToSize();
    }

    public static void Pixel(Graphics G, Color C, int X, int Y)
    {
        G.FillRectangle(new SolidBrush(C), X, Y, 1, 1);
    }

    public static void Gradient(Graphics G, Color C1, Color C2, int X, int Y, int Width, int Height, float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        G.FillRectangle(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Gradient(Graphics G, Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        G.FillRectangle(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static LinearGradientBrush Gradient(Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        return new LinearGradientBrush(R, C1, C2, Angle);
    }

    public static LinearGradientBrush Gradient(Color C1, Color C2, int X, int Y, int Width, int Height,
        float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        return new LinearGradientBrush(R, C1, C2, Angle);
    }

    public static void Radial(Graphics G, Color C1, Color C2, int X, int Y, int Width, int Height, float Angle = 90f)
    {
        var R = new Rectangle(X, Y, Width, Height);
        G.FillEllipse(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Radial(Graphics G, Color C1, Color C2, Rectangle R, float Angle = 90f)
    {
        G.FillEllipse(new LinearGradientBrush(R, C1, C2, Angle), R);
    }

    public static void Image(Graphics G, Image I, int X, int Y)
    {
        if (I == null) return;
        G.DrawImage(I, X, Y, I.Width, I.Height);
    }

    public static void Image(Graphics G, Image I, Point Location)
    {
        if (I == null) return;
        G.DrawImage(I, Location.X, Location.Y, I.Width, I.Height);
    }

    public static void Text(Graphics G, Brush B, int X, int Y, Font Font, string Text)
    {
        if (Text.Length == 0) return;
        G.DrawString(Text, Font, B, X, Y);
    }

    public static void Text(Graphics G, Brush B, Point P, Font Font, string Text)
    {
        if (Text.Length == 0) return;
        G.DrawString(Text, Font, B, P);
    }

    public static void Borders(Graphics G, Pen P, int X, int Y, int W, int H, int Offset = 0)
    {
        G.DrawRectangle(P, X + Offset, Y + Offset, (W - (Offset * 2)) - 1, (H - (Offset * 2) - 1));
    }

    public static void Borders(Graphics G, Pen P, Rectangle R, int Offset = 0)
    {
        G.DrawRectangle(P, R.X + Offset, R.Y + Offset, (R.Width - (Offset * 2)) - 1, (R.Height - (Offset * 2) - 1));
    }
}


internal class NiceBrightTheme : ThemeContainer154
{
    // Fields
    private Color Accent = Color.FromArgb(135, 177, 74);
    private Color Border = Color.FromArgb(225, 225, 225);
    private Color TextColor = Color.FromArgb(23, 23, 23);
    private Color TitleBottom = Color.FromArgb(170, 170, 170);
    private Color TitleTop = Color.FromArgb(245, 245, 245);
    private readonly Color Base = Color.FromArgb(232, 232, 232);
    private bool _ShowIcon;
    // Methods
    public NiceBrightTheme()
    {
        Header = 30;
        SetColor("Titlebar Gradient Top", TitleTop);
        SetColor("Titlebar Gradient Bottom", TitleBottom);
        SetColor("Text", TextColor);
        SetColor("Accent", Accent);
        SetColor("Border", Border);
        TransparencyKey = Color.Fuchsia;
        BackColor = Base;
        Font = new Font("Segoe UI", 9f);
    }

    public bool ShowIcon
    {
        get { return _ShowIcon; }
        set
        {
            _ShowIcon = value;
            Invalidate();
        }
    }

    protected override void ColorHook()
    {
        TitleTop = GetColor("Titlebar Gradient Top");
        TitleBottom = GetColor("Titlebar Gradient Bottom");
        TextColor = GetColor("Text");
        Accent = GetColor("Accent");
        Border = GetColor("Border");
    }

    protected override void PaintHook()
    {
        base.G.Clear(Border);
        var rect = new Rectangle(1, 1, Width - 2, 0x23);
        var brush = new LinearGradientBrush(rect, TitleTop, TitleBottom, 90f);
        base.G.FillPath(brush, CreateRound(1, 1, Width - 2, 0x23, 7));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), CreateRound(1, 1, Width - 3, 0x23, 7));
        base.G.FillPath(new SolidBrush(BackColor), CreateRound(1, 0x20, Width - 2, Height - 0x21, 7));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White), 1f), CreateRound(1, 0x20, Width - 3, Height - 0x22, 7));
        rect = new Rectangle(1, 0x20, Width - 2, 3);
        base.G.FillRectangle(new SolidBrush(Border), rect);
        var point = new Point(1, 0x1f);
        var point2 = new Point(Width - 2, 0x1f);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point, point2);
        var blend = new ColorBlend(3);
        blend.Colors = new[] { Color.FromArgb(30, 30, 30), Accent, Color.FromArgb(30, 30, 30) };
        blend.Positions = new[] { 0f, 0.5f, 1f };
        rect = new Rectangle(1, 0x21, Width - 2, 2);
        DrawGradient(blend, rect, 0f);
        point2 = new Point(1, 0x23);
        point = new Point(Width - 2, 0x23);
        base.G.DrawLine(new Pen(BackColor), point2, point);
        point2 = new Point(1, 0x23);
        point = new Point(Width - 2, 0x23);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point2, point);
        if (_ShowIcon)
        {
            rect = new Rectangle(11, 8, 0x10, 0x10);
            base.G.DrawIcon(FindForm().Icon, rect);
            point2 = new Point(0x20, 8);
            base.G.DrawString(FindForm().Text, Font, new SolidBrush(TextColor), point2);
        }
        else
        {
            point2 = new Point(13, 8);
            base.G.DrawString(FindForm().Text, Font, new SolidBrush(TextColor), point2);
        }
        DrawPixel(Color.Fuchsia, 0, 0);
        DrawPixel(Color.Fuchsia, 1, 0);
        DrawPixel(Color.Fuchsia, 2, 0);
        DrawPixel(Color.Fuchsia, 3, 0);
        DrawPixel(Color.Fuchsia, 0, 1);
        DrawPixel(Color.Fuchsia, 0, 2);
        DrawPixel(Color.Fuchsia, 0, 3);
        DrawPixel(Color.Fuchsia, 1, 1);
        DrawPixel(Color.Fuchsia, Width - 1, 0);
        DrawPixel(Color.Fuchsia, Width - 2, 0);
        DrawPixel(Color.Fuchsia, Width - 3, 0);
        DrawPixel(Color.Fuchsia, Width - 4, 0);
        DrawPixel(Color.Fuchsia, Width - 1, 1);
        DrawPixel(Color.Fuchsia, Width - 1, 2);
        DrawPixel(Color.Fuchsia, Width - 1, 3);
        DrawPixel(Color.Fuchsia, Width - 2, 1);
        DrawPixel(Color.Fuchsia, 0, Height);
        DrawPixel(Color.Fuchsia, 1, Height);
        DrawPixel(Color.Fuchsia, 2, Height);
        DrawPixel(Color.Fuchsia, 3, Height);
        DrawPixel(Color.Fuchsia, 0, Height - 1);
        DrawPixel(Color.Fuchsia, 0, Height - 2);
        DrawPixel(Color.Fuchsia, 0, Height - 3);
        DrawPixel(Color.Fuchsia, 1, Height - 1);
        DrawPixel(Color.Fuchsia, Width - 1, Height);
        DrawPixel(Color.Fuchsia, Width - 2, Height);
        DrawPixel(Color.Fuchsia, Width - 3, Height);
        DrawPixel(Color.Fuchsia, Width - 4, Height);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 1);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 2);
        DrawPixel(Color.Fuchsia, Width - 1, Height - 3);
        DrawPixel(Color.Fuchsia, Width - 2, Height - 1);
    }

    // Properties
}

#endregion

internal class NiceBrightTheme_TwoButtons : ThemeControl154
{
    private Color Accent = Color.FromArgb(135, 177, 74);
    private readonly Color Border = Color.FromArgb(225, 225, 225);
    private readonly Color TextColor = Color.FromArgb(23, 23, 23);
    private readonly Color TitleBottom = Color.FromArgb(170, 170, 170);
    private readonly Color TitleTop = Color.FromArgb(245, 245, 245);
    private Color Base = Color.FromArgb(232, 232, 232);
    // Fields
    private Color G1;
    private Color G2;
    private Color G3;
    private Color I = Color.FromArgb(135, 177, 74);
    private Color O;
    private int X;


    // Methods
    public NiceBrightTheme_TwoButtons()
    {
        SetColor("Gradient Top", TitleTop);
        SetColor("Gradient Bottom", TitleBottom);
        SetColor("Icons", TextColor);
        SetColor("Outline", 90, Border);
        var size = new Size(0x35, 0x1c);
        Size = size;
        Anchor = AnchorStyles.Right | AnchorStyles.Top;
    }

    protected override void ColorHook()
    {
        G1 = GetColor("Gradient Top");
        G2 = GetColor("Gradient Bottom");
        //G3 = GetColor("Gradient Bottom");
        I = GetColor("Icons");
        O = GetColor("Outline");
    }

    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        if (X < 30)
        {
            FindForm().WindowState = FormWindowState.Minimized;
        }
        else if (X > 30)
        {
            FindForm().Close();
        }
    }

    protected override void OnLocationChanged(EventArgs e)
    {
        base.OnLocationChanged(e);
        Top = 2;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Invalidate();
    }

    protected override void PaintHook()
    {
        base.G.Clear(BackColor);
        var rect = new Rectangle(0, 0, Width, Height);
        var brush = new LinearGradientBrush(rect, G1, G2, 90f);
        var blend = new ColorBlend(2);
        blend.Colors = new[] { G1, G2 };
        blend.Positions = new[] { 0f, 1f };
        brush.InterpolationColors = blend;
        rect = new Rectangle(0, 0, Width, Height);
        base.G.FillRectangle(brush, rect);
        base.G.SmoothingMode = SmoothingMode.HighQuality;
        if (base.State == MouseState.Over)
        {
            if (X < 30)
            {
                base.G.FillPath(new SolidBrush(Color.FromArgb(20, Color.Black)), CreateRound(4, 4, 0x16, 0x12, 6));
                base.G.DrawPath(new Pen(O), CreateRound(4, 4, 0x16, 0x12, 6));
            }
            else if (X > 30)
            {
                base.G.FillPath(new SolidBrush(Color.FromArgb(20, Color.Black)), CreateRound(0x1b, 4, 0x17, 0x12, 6));
                base.G.DrawPath(new Pen(O), CreateRound(0x1b, 4, 0x17, 0x12, 6));
            }
        }
        else if (base.State == MouseState.Down)
        {
            if (X < 30)
            {
                base.G.FillPath(new SolidBrush(Color.FromArgb(70, Color.Black)), CreateRound(4, 4, 0x16, 0x12, 6));
                base.G.DrawPath(new Pen(O), CreateRound(4, 4, 0x16, 0x12, 6));
            }
            else if (X > 30)
            {
                base.G.FillPath(new SolidBrush(Color.FromArgb(70, Color.Black)), CreateRound(0x1b, 4, 0x17, 0x12, 6));
                base.G.DrawPath(new Pen(O), CreateRound(0x1b, 4, 0x17, 0x12, 6));
            }
        }
        var point = new Point(8, 7);
        base.G.DrawString("0", new Font("Marlett", 10f), new SolidBrush(I), point);
        point = new Point(0x1f, 7);
        base.G.DrawString("r", new Font("Marlett", 10f), new SolidBrush(I), point);
    }
}

internal class NiceBrightTheme_GroupBox : ThemeContainer154
{
    private Color Accent = Color.FromArgb(135, 177, 74);
    private readonly Color Border = Color.FromArgb(225, 225, 225);
    private readonly Color TextColor = Color.FromArgb(23, 23, 23);
    private readonly Color TitleBottom = Color.FromArgb(170, 170, 170);
    private readonly Color TitleTop = Color.FromArgb(245, 245, 245);
    private Color Base = Color.FromArgb(232, 232, 232);
    // Fields
    private Color B;
    private Color G1;
    private Color G2;
    private Color TC;

    // Methods
    public NiceBrightTheme_GroupBox()
    {
        ControlMode = true;
        SetColor("Gradient Top", TitleTop);
        SetColor("Gradient Bottom", TitleBottom);
        SetColor("Text", TextColor);
        SetColor("Border", Border);
    }

    protected override void ColorHook()
    {
        G1 = GetColor("Gradient Top");
        G2 = GetColor("Gradient Bottom");
        TC = GetColor("Text");
        B = GetColor("Border");
    }

    protected override void PaintHook()
    {
        base.G.Clear(BackColor);
        base.G.SmoothingMode = SmoothingMode.HighQuality;
        base.G.DrawPath(new Pen(B), CreateRound(0, 0, Width - 1, Height - 1, 7));
        var rect = new Rectangle(0, 0, Width - 1, 0x1b);
        var brush = new LinearGradientBrush(rect, G1, G2, 90f);
        base.G.FillPath(brush, CreateRound(0, 0, Width - 1, 0x1b, 7));
        base.G.DrawPath(new Pen(B), CreateRound(0, 0, Width - 1, 0x1b, 7));
        base.G.SmoothingMode = SmoothingMode.None;
        rect = new Rectangle(1, 0x18, Width - 2, 10);
        base.G.FillRectangle(new SolidBrush(BackColor), rect);
        var point = new Point(0, 0x18);
        var point2 = new Point(Width, 0x18);
        base.G.DrawLine(new Pen(B), point, point2);
        point2 = new Point(2, 0x17);
        point = new Point(Width - 3, 0x17);
        base.G.DrawLine(new Pen(Color.FromArgb(15, Color.Black)), point2, point);
        point2 = new Point(7, 5);
        base.G.DrawString(Text, Font, new SolidBrush(TC), point2);
        base.G.SmoothingMode = SmoothingMode.HighQuality;
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.Black)), CreateRound(1, 1, Width - 3, Height - 3, 7));
    }
}

internal class NiceBrightTheme_ComboBox : ComboBox
{
    private Color Accent = Color.FromArgb(135, 177, 74);
    private Color Border = Color.FromArgb(225, 225, 225);
    private readonly Color TextColor = Color.FromArgb(23, 23, 23);
    private readonly Color TitleBottom = Color.FromArgb(170, 170, 170);
    private readonly Color TitleTop = Color.FromArgb(245, 245, 245);
    private readonly Color Base = Color.FromArgb(232, 232, 232);
    // Fields
    private GraphicsPath CreateRoundPath;
    private Rectangle CreateRoundRectangle;
    private int X;

    // Methods
    public NiceBrightTheme_ComboBox()
    {
        base.DropDownClosed += GhostComboBox_DropDownClosed;
        base.TextChanged += GhostCombo_TextChanged;
        SetStyle(
            ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint, true);
        ForeColor = TextColor;
        BackColor = Base;
        DrawMode = DrawMode.OwnerDrawFixed;
        ItemHeight = 0x11;
        DropDownStyle = ComboBoxStyle.DropDownList;
    }


    public GraphicsPath CreateRound(Rectangle r, int slope)
    {
        CreateRoundPath = new GraphicsPath(FillMode.Winding);
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270f, 90f);
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0f, 90f);
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90f, 90f);
        CreateRoundPath.CloseFigure();
        return CreateRoundPath;
    }

    public GraphicsPath CreateRound(int x, int y, int width, int height, int slope)
    {
        CreateRoundRectangle = new Rectangle(x, y, width, height);
        return CreateRound(CreateRoundRectangle, slope);
    }

    private void GhostCombo_TextChanged(object sender, EventArgs e)
    {
        Invalidate();
    }

    private void GhostComboBox_DropDownClosed(object sender, EventArgs e)
    {
        DropDownStyle = ComboBoxStyle.Simple;
        Application.DoEvents();
        DropDownStyle = ComboBoxStyle.DropDownList;
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        if (e.Index >= 0)
        {
            var rectangle = new Rectangle
            {
                X = e.Bounds.X,
                Y = e.Bounds.Y,
                Width = e.Bounds.Width - 1,
                Height = e.Bounds.Height - 1
            };
            e.DrawBackground();
            if ((e.State ==
                 (DrawItemState.NoFocusRect | DrawItemState.NoAccelerator | DrawItemState.Focus | DrawItemState.Selected)) |
                (e.State == (DrawItemState.Focus | DrawItemState.Selected)))
            {
                e.Graphics.FillRectangle(new SolidBrush(TitleTop), e.Bounds);
                e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, Brushes.SlateGray, e.Bounds.X, e.Bounds.Y);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(BackColor), e.Bounds);
                e.Graphics.DrawString(Items[e.Index].ToString(), e.Font, Brushes.White, e.Bounds.X, e.Bounds.Y);
            }
            base.OnDrawItem(e);
        }
    }

    protected override void OnDropDownClosed(EventArgs e)
    {
        base.OnDropDownClosed(e);
        X = -1;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        X = -1;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (DropDownStyle != ComboBoxStyle.DropDownList)
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
        }
        var image = new Bitmap(Width, Height);
        Graphics graphics = Graphics.FromImage(image);
        graphics.Clear(BackColor);
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        var brush = new LinearGradientBrush(rect, TitleTop, TitleBottom, 90f);
        graphics.FillPath(brush, CreateRound(0, 0, Width - 1, Height - 1, 5));
        if (X > (Width - 0x1a))
        {
            rect = new Rectangle(Width - 0x19, 2, 0x18, Height - 4);
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(5, Color.White)), rect);
        }
        graphics.DrawPath(Pens.DarkGray, CreateRound(0, 0, Width - 1, Height - 1, 5)); //(Border)
        graphics.DrawPath(new Pen(Color.FromArgb(15, Color.White)), CreateRound(1, 1, Width - 3, Height - 3, 5));
        var point = new Point(Width - 0x19, 0);
        var point2 = new Point(Width - 0x19, Height);
        graphics.DrawLine(Pens.DarkGray, point, point2); //Pipe
        point2 = new Point(Width - 0x18, 2);
        point = new Point(Width - 0x18, Height - 3);
        graphics.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point2, point);
        point2 = new Point(Width - 0x1a, 2);
        point = new Point(Width - 0x1a, Height - 3);
        graphics.DrawLine(new Pen(Color.FromArgb(15, Color.White)), point2, point);
        var num = (int)Math.Round(graphics.MeasureString(" ... ", Font).Height);
        if (SelectedIndex != -1)
        {
            graphics.DrawString((Items[SelectedIndex]).ToString(), Font, new SolidBrush(ForeColor), 4f,
                (Height / 2) - (num / 2));
        }
        else if ((Items != null) & (Items.Count > 0))
        {
            graphics.DrawString((Items[0]).ToString(), Font, new SolidBrush(ForeColor), 4f, (Height / 2) - (num / 2));
        }
        else
        {
            graphics.DrawString(" ... ", Font, new SolidBrush(ForeColor), 4f, (Height / 2) - (num / 2));
        }
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        var pointArray = new Point[3];
        point2 = new Point(Width - 0x12, 9);
        pointArray[0] = point2;
        point = new Point(Width - 10, 9);
        pointArray[1] = point;
        var point3 = new Point(Width - 14, 14);
        pointArray[2] = point3;
        Point[] points = pointArray;
        graphics.FillPolygon(new SolidBrush(Color.FromArgb(170, 170, 170)), points);
        e.Graphics.DrawImage((Image)image.Clone(), 0, 0);

        graphics.Dispose();
        image.Dispose();
    }

    public Point[] Triangle(Point Location, Size Size)
    {
        return new[]
        {
            Location, new Point(Location.X + Size.Width, Location.Y),
            new Point(Location.X + (Size.Width/2), Location.Y + Size.Height), Location
        };
    }
}

internal class NiceBrightButton : ThemeControl154
{
    private Color Accent = Color.FromArgb(135, 177, 74);
    private Color Border = Color.FromArgb(225, 225, 225);
    private readonly Color TextColor = Color.FromArgb(23, 23, 23);
    private readonly Color TitleBottom = Color.FromArgb(170, 170, 170);
    private readonly Color TitleTop = Color.FromArgb(245, 245, 245);
    private Color Base = Color.FromArgb(232, 232, 232);
    // Fields
    private Color G1;
    private Color G2;
    private Color TC;

    // Methods
    public NiceBrightButton()
    {
        SetColor("Gradient Top", TitleTop);
        SetColor("Gradient Bottom", TitleBottom);
        SetColor("Text", TextColor);
    }

    protected override void ColorHook()
    {
        G1 = GetColor("Gradient Top");
        G2 = GetColor("Gradient Bottom");
        TC = GetColor("Text");
    }

    protected override void PaintHook()
    {
        Rectangle rectangle;
        base.G.Clear(BackColor);
        base.G.SmoothingMode = SmoothingMode.HighQuality;
        switch (base.State)
        {
            case MouseState.None:
                {
                    rectangle = new Rectangle(0, 0, Width - 1, Height - 1);
                    var brush = new LinearGradientBrush(rectangle, G1, G2, 90f);
                    base.G.FillPath(brush, CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.DrawPath(Pens.DarkGray, CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White)), CreateRound(1, 1, Width - 3, Height - 3, 5));
                    break;
                }
            case MouseState.Over:
                {
                    rectangle = new Rectangle(0, 0, Width - 1, Height - 1);
                    var brush2 = new LinearGradientBrush(rectangle, G1, G2, 90f);
                    base.G.FillPath(brush2, CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.FillPath(new SolidBrush(Color.FromArgb(7, Color.White)),
                        CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.DrawPath(Pens.DarkGray, CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White)), CreateRound(1, 1, Width - 3, Height - 3, 5));
                    break;
                }
            case MouseState.Down:
                {
                    rectangle = new Rectangle(0, 0, Width - 1, Height - 1);
                    var brush3 = new LinearGradientBrush(rectangle, G1, G2, 90f);
                    base.G.FillPath(brush3, CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.FillPath(new SolidBrush(Color.FromArgb(20, Color.Black)),
                        CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.DrawPath(Pens.DarkGray, CreateRound(0, 0, Width - 1, Height - 1, 5));
                    base.G.DrawPath(new Pen(Color.FromArgb(15, Color.White)), CreateRound(1, 1, Width - 3, Height - 3, 5));
                    break;
                }
        }
        rectangle = new Rectangle(0, 0, Width - 1, Height);
        var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        base.G.DrawString(Text, Font, new SolidBrush(TC), rectangle, format);
    }
}

[DefaultEvent("TextChanged")]
internal class NiceBright_TextBox : ThemeControl154
{
    private Color Accent = Color.FromArgb(135, 177, 74);
    private Color Border = Color.DarkGray;
    private readonly Color TextColor = Color.FromArgb(23, 23, 23);
    private Color TitleBottom = Color.FromArgb(170, 170, 170);
    private Color TitleTop = Color.FromArgb(245, 245, 245);
    private readonly Color Basecolor = Color.FromArgb(232, 232, 232);
    // Fields
    private readonly TextBox Base = new TextBox();
    private Color Background;

    private int _MaxLength = 0x7fff;
    private bool _Multiline;
    private bool _ReadOnly;
    private HorizontalAlignment _TextAlign = HorizontalAlignment.Left;
    private bool _UseSystemPasswordChar;

    // Methods
    public NiceBright_TextBox()
    {
        Base.Font = Font;
        Base.Text = Text;
        Base.MaxLength = _MaxLength;
        Base.Multiline = _Multiline;
        Base.ReadOnly = _ReadOnly;
        Base.UseSystemPasswordChar = _UseSystemPasswordChar;
        Base.BorderStyle = BorderStyle.None;
        var point = new Point(4, 4);
        Base.Location = point;
        Base.Width = Width - 10;
        if (_Multiline)
        {
            Base.Height = Height - 11;
        }
        else
        {
            LockHeight = Base.Height + 11;
        }
        Base.TextChanged += OnBaseTextChanged;
        Base.KeyDown += OnBaseKeyDown;
        SetColor("Text", TextColor);
        SetColor("Background", Basecolor);
        SetColor("Border", Border);
    }

    // Properties
    public override Font Font
    {
        get { return base.Font; }
        set
        {
            base.Font = value;
            if (Base != null)
            {
                Base.Font = value;
                var point = new Point(3, 5);
                Base.Location = point;
                Base.Width = Width - 6;
                if (!_Multiline)
                {
                    LockHeight = Base.Height + 11;
                }
            }
        }
    }

    public int MaxLength
    {
        get { return _MaxLength; }
        set
        {
            _MaxLength = value;
            if (Base != null)
            {
                Base.MaxLength = value;
            }
        }
    }

    public bool Multiline
    {
        get { return _Multiline; }
        set
        {
            _Multiline = value;
            if (Base != null)
            {
                Base.Multiline = value;
                if (value)
                {
                    LockHeight = 0;
                    Base.Height = Height - 11;
                }
                else
                {
                    LockHeight = Base.Height + 11;
                }
            }
        }
    }

    public string PasswordChar
    {
        get { return Base.PasswordChar.ToString(); }
        set { Base.PasswordChar = (value).ToCharArray()[0]; }
    }

    public bool ReadOnly
    {
        get { return _ReadOnly; }
        set
        {
            _ReadOnly = value;
            if (Base != null)
            {
                Base.ReadOnly = value;
            }
        }
    }

    public override string Text
    {
        get { return base.Text; }
        set
        {
            base.Text = value;
            if (Base != null)
            {
                Base.Text = value;
            }
        }
    }

    public HorizontalAlignment TextAlign
    {
        get { return _TextAlign; }
        set
        {
            _TextAlign = value;
            if (Base != null)
            {
                Base.TextAlign = value;
            }
        }
    }

    public bool UseSystemPasswordChar
    {
        get { return _UseSystemPasswordChar; }
        set
        {
            _UseSystemPasswordChar = value;
            if (Base != null)
            {
                Base.UseSystemPasswordChar = value;
            }
        }
    }

    protected override void ColorHook()
    {
        Background = GetColor("Background");
        Border = GetColor("Border");
        Base.ForeColor = GetColor("Text");
        Base.BackColor = Background;
    }

    private void OnBaseKeyDown(object sender, KeyEventArgs e)
    {
        if (((!e.Control || (e.KeyCode != Keys.A)) ? 0 : 1) != 0)
        {
            Base.SelectAll();
            e.SuppressKeyPress = true;
        }
    }

    private void OnBaseTextChanged(object sender, EventArgs e)
    {
        Text = Base.Text;
    }

    protected override void OnCreation()
    {
        if (!Controls.Contains(Base))
        {
            Controls.Add(Base);
        }
    }

    protected override void OnResize(EventArgs e)
    {
        var point = new Point(5, 5);
        Base.Location = point;
        Base.Width = Width - 10;
        if (_Multiline)
        {
            Base.Height = Height - 5;
        }
        base.OnResize(e);
    }

    protected override void PaintHook()
    {
        base.G.Clear(BackColor);
        base.G.SmoothingMode = SmoothingMode.HighQuality;
        base.G.FillPath(new SolidBrush(Background), CreateRound(0, 0, Width - 1, Height - 1, 6));
        base.G.DrawPath(new Pen(Color.FromArgb(15, Color.Black)), CreateRound(1, 1, Width - 3, Height - 3, 6));
        base.G.DrawPath(new Pen(Border), CreateRound(0, 0, Width - 1, Height - 1, 6));
    }
}

public class sexyWhite_Label : Control
{
    public sexyWhite_Label()
    {
        SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        SetStyle(ControlStyles.UserPaint, true);
        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(30, 30, 30);
        DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var B = new Bitmap(Width, Height);
        Graphics G = Graphics.FromImage(B);
        var ClientRectangle = new Rectangle(0, 0, Width - 1, Height - 1);
        base.OnPaint(e);
        G.Clear(BackColor);
        var drawFont = new Font("Tahoma", 9, FontStyle.Bold);
        var format = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
        G.CompositingQuality = CompositingQuality.HighQuality;
        G.SmoothingMode = SmoothingMode.HighQuality;
        G.DrawString(Text, drawFont, new SolidBrush(Color.DarkGray), new Rectangle(1, 0, Width - 1, Height - 1),
            format);
        G.DrawString(Text, drawFont, new SolidBrush(Color.FromArgb(30, 30, 30)),
            new Rectangle(0, -1, Width - 1, Height - 1), format);
        e.Graphics.DrawImage((Image)B.Clone(), 0, 0);
        G.Dispose();
        B.Dispose();
    }
}

internal class sexyWhite_TabView : TabControl
{
    private Color Accent = Color.FromArgb(135, 177, 74);
    // private Color Border = Color.DarkGray;
    private Color TextColor = Color.FromArgb(23, 23, 23);
    private Color TitleBottom = Color.FromArgb(170, 170, 170);
    private readonly Color TitleTop = Color.FromArgb(245, 245, 245);
    private readonly Color Basecolor = Color.FromArgb(232, 232, 232);


    // Describes the brush's color using RGB values. 
    // Each value has a range of 0-255.

    // Fields
    private readonly Pen Border = Pens.DarkGray;

    // Methods
    public sexyWhite_TabView()
    {
        Border = Pens.DarkGray;
        SetStyle(
            ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint, true);
        DoubleBuffered = true;
        SizeMode = TabSizeMode.Fixed;
        var size = new Size(0x2c, 0x88);
        ItemSize = size;
    }


    protected override void CreateHandle()
    {
        base.CreateHandle();
        Alignment = TabAlignment.Left;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Rectangle tabRect;
        Point point4;
        Point point5;
        Size size3;
        StringFormat format;
        var image = new Bitmap(Width, Height);
        Graphics graphics = Graphics.FromImage(image);
        try
        {
            SelectedTab.BackColor = Basecolor;
        }
        catch
        {
        }
        graphics.Clear(Color.FromArgb(210, 210, 210));
        var point = new Point(ItemSize.Height + 3, 0);
        var location = new Point(ItemSize.Height + 3, 0x3e7);
        graphics.DrawLine(Border, point, location);
        Size itemSize = ItemSize;
        location = new Point(itemSize.Height + 2, 0);
        point = new Point(ItemSize.Height + 2, 0x3e7);
        graphics.DrawLine(new Pen(Color.FromArgb(15, Color.Black)), location, point);
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        graphics.DrawRectangle(Border, rect);
        rect = new Rectangle(1, 1, Width - 3, Height - 3);
        graphics.DrawRectangle(new Pen(Color.FromArgb(15, Color.Black)), rect);
        int num = TabCount - 1;
        int index = 0;
    Label_0147:
        if (index > num)
        {
            e.Graphics.DrawImage((Image)image.Clone(), 0, 0);
            graphics.Dispose();
            image.Dispose();
            return;
        }
        if (index == SelectedIndex)
        {
            Rectangle rectangle2;
            Point point3;
            if (index == -1)
            {
                point = GetTabRect(index).Location;
                point3 = new Point(GetTabRect(index).Location.X - 2, point.Y - 2);
                itemSize = new Size(GetTabRect(index).Width + 3, GetTabRect(index).Height + 1);
                rectangle2 = new Rectangle(point3, itemSize);
            }
            else
            {
                tabRect = GetTabRect(index);
                point = new Point(tabRect.Location.X - 2, GetTabRect(index).Location.Y - 2);
                itemSize = new Size(GetTabRect(index).Width + 3, GetTabRect(index).Height);
                rectangle2 = new Rectangle(point, itemSize);
            }
            var blend = new ColorBlend();
            blend.Colors = new[] { TitleTop, Color.FromArgb(135, 177, 74), TitleTop };
            blend.Positions = new[] { 0f, 0.5f, 1f };
            var brush = new LinearGradientBrush(rectangle2, Color.Black, Color.Black, 90f)
            {
                InterpolationColors = blend
            };
            graphics.FillRectangle(brush, rectangle2);
            graphics.DrawRectangle(Border, rectangle2);
            tabRect = new Rectangle(rectangle2.Location.X + 1, rectangle2.Location.Y + 1, rectangle2.Width - 2,
                rectangle2.Height - 2);
            graphics.DrawRectangle(new Pen(Color.FromArgb(15, Color.Black)), tabRect);
            location = GetTabRect(index).Location;
            point = new Point(GetTabRect(index).Location.X - 2, location.Y - 2);
            itemSize = new Size(GetTabRect(index).Width + 3, GetTabRect(index).Height + 1);
            rectangle2 = new Rectangle(point, itemSize);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            var pointArray = new Point[3];
            itemSize = ItemSize;
            tabRect = GetTabRect(index);
            point3 = tabRect.Location;
            location = new Point(itemSize.Height - 3, point3.Y + 20);
            pointArray[0] = location;
            point = GetTabRect(index).Location;
            point4 = new Point(ItemSize.Height + 4, point.Y + 14);
            pointArray[1] = point4;
            size3 = ItemSize;
            point5 = new Point(size3.Height + 4, GetTabRect(index).Location.Y + 0x1b);
            pointArray[2] = point5;
            Point[] points = pointArray;
            graphics.DrawPolygon(new Pen(Color.FromArgb(15, Color.Black), 3f), points);
            graphics.FillPolygon(new SolidBrush(Color.FromArgb(120, 120, 110)), points);
            graphics.DrawPolygon(Border, points);
            if (ImageList != null)
            {
                try
                {
                    if (ImageList.Images[TabPages[index].ImageIndex] != null)
                    {
                        point5 = rectangle2.Location;
                        point4 = new Point(point5.X + 8, rectangle2.Location.Y + 6);
                        graphics.DrawImage(ImageList.Images[TabPages[index].ImageIndex], point4);
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString("      " + TabPages[index].Text, Font, Brushes.Black, rectangle2, format);
                    }
                    else
                    {
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString(TabPages[index].Text, Font, Brushes.Black, rectangle2, format);
                    }
                    goto Label_09D5;
                }
                catch
                {
                    format = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };
                    graphics.DrawString(TabPages[index].Text, Font, Brushes.Black, rectangle2, format);
                    goto Label_09D5;
                }
            }
            format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            graphics.DrawString(TabPages[index].Text, Font, Brushes.Black, rectangle2, format);
        }
        else
        {
            tabRect = GetTabRect(index);
            point5 = tabRect.Location;
            Point point6 = GetTabRect(index).Location;
            point4 = new Point(point5.X - 1, point6.Y - 1);
            size3 = new Size(GetTabRect(index).Width + 2, GetTabRect(index).Height);
            var layoutRectangle = new Rectangle(point4, size3);
            point5 = new Point(layoutRectangle.Right, layoutRectangle.Top);
            point6 = new Point(layoutRectangle.Right, layoutRectangle.Bottom);
            graphics.DrawLine(Border, point5, point6);
            point5 = new Point(layoutRectangle.Right - 1, layoutRectangle.Top);
            point6 = new Point(layoutRectangle.Right - 1, layoutRectangle.Bottom);
            graphics.DrawLine(new Pen(TitleTop), point5, point6);
            if (ImageList != null)
            {
                try
                {
                    if (ImageList.Images[TabPages[index].ImageIndex] != null)
                    {
                        point5 = layoutRectangle.Location;
                        point4 = new Point(point5.X + 8, layoutRectangle.Location.Y + 6);
                        graphics.DrawImage(ImageList.Images[TabPages[index].ImageIndex], point4);
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString("      " + TabPages[index].Text, Font,
                            new SolidBrush(Color.FromArgb(170, 170, 170)), layoutRectangle, format);
                    }
                    else
                    {
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString(TabPages[index].Text, Font, new SolidBrush(Color.FromArgb(170, 170, 170)),
                            layoutRectangle, format);
                    }
                    goto Label_09D5;
                }
                catch
                {
                    format = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };
                    graphics.DrawString(TabPages[index].Text, Font, new SolidBrush(Color.FromArgb(170, 170, 170)),
                        layoutRectangle, format);
                    goto Label_09D5;
                }
            }
            format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            graphics.DrawString(TabPages[index].Text, Font, new SolidBrush(Color.FromArgb(170, 170, 170)),
                layoutRectangle, format);
        }
    Label_09D5:
        index++;
        goto Label_0147;
    }

    public Brush ToBrush(Color color)
    {
        return new SolidBrush(color);
    }

    public Pen ToPen(Color color)
    {
        return new Pen(color);
    }
}

public sealed class sexyAnimatedWhiteTab : TabControl
{
    private Color Accent = Color.FromArgb(135, 177, 74);
    // private Color Border = Color.DarkGray;
    private Color TextColor = Color.FromArgb(23, 23, 23);
    private readonly Color TitleBottom = Color.FromArgb(170, 170, 170);
    private readonly Color TitleTop = Color.FromArgb(245, 245, 245);
    private readonly Color Basecolor = Color.FromArgb(232, 232, 232);

    private int _oldIndex = 1;
    private bool _transitionEnabled = true;
    private int _transitionSpeed = 20;
    private readonly Pen Border = Pens.DarkGray;

    /// <summary>
    ///     Creates a new instance of the <see cref="AnimatedVerticalTabControl" />
    /// </summary>
    public sexyAnimatedWhiteTab()
    {
        SetStyle(
            ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint, true);
        DoubleBuffered = true;
        SizeMode = TabSizeMode.Fixed;
        var size = new Size(0x2c, 0x88);
        ItemSize = size;
    }

    /// <summary>
    ///     The speed of the transition.
    /// </summary>
    public int TransitionSpeed
    {
        get { return _transitionSpeed; }
        set { _transitionSpeed = value; }
    }

    /// <summary>
    ///     Enables or disables transitions when switching between <see cref="TabPage" />s.
    /// </summary>
    public bool TransitionEnabled
    {
        get { return _transitionEnabled; }
        set { _transitionEnabled = value; }
    }

    /// <summary>
    ///     Raises the <see cref="Control.CreateControl" /> method.
    /// </summary>
    protected override void OnCreateControl()
    {
        if (!DesignMode)
        {
            TransitionEnabled = false;
            for (int i = 0; i < TabPages.Count; i++)
            {
                SelectedIndex = i;
            }
            TransitionEnabled = true;
            SelectedIndex = 0;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Rectangle tabRect;
        Point point4;
        Point point5;
        Size size3;
        StringFormat format;
        var image = new Bitmap(Width, Height);
        Graphics graphics = Graphics.FromImage(image);
        try
        {
            SelectedTab.BackColor = Basecolor;
        }
        catch
        {
        }
        graphics.Clear(Color.FromArgb(210, 210, 210));
        var point = new Point(ItemSize.Height + 3, 0);
        var location = new Point(ItemSize.Height + 3, 0x3e7);
        //graphics.DrawLine(Border, point, location);
        Size itemSize = ItemSize;
        location = new Point(itemSize.Height + 2, 0);
        point = new Point(ItemSize.Height + 2, 0x3e7);
        //graphics.DrawLine(new Pen(Color.FromArgb(15, Color.Black)), location, point);
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        graphics.DrawRectangle(Border, rect);
        rect = new Rectangle(1, 1, Width - 3, Height - 3);
        graphics.DrawRectangle(new Pen(Color.FromArgb(15, Color.Black)), rect);
        int num = TabCount - 1;
        int index = 0;
    Label_0147:
        if (index > num)
        {
            e.Graphics.DrawImage((Image)image.Clone(), 0, 0);
            graphics.Dispose();
            image.Dispose();
            return;
        }
        if (index == SelectedIndex)
        {
            Rectangle rectangle2;
            Point point3;
            if (index == -1)
            {
                point = GetTabRect(index).Location;
                point3 = new Point(GetTabRect(index).Location.X - 2, point.Y - 2);
                itemSize = new Size(GetTabRect(index).Width + 3, GetTabRect(index).Height + 1);
                rectangle2 = new Rectangle(point3, itemSize);
            }
            else
            {
                tabRect = GetTabRect(index);
                point = new Point(tabRect.Location.X - 2, GetTabRect(index).Location.Y - 2);
                itemSize = new Size(GetTabRect(index).Width + 3, GetTabRect(index).Height);
                rectangle2 = new Rectangle(point, itemSize);
            }
            var blend = new ColorBlend();
            blend.Colors = new[] { TitleTop, Color.FromArgb(135, 177, 74), TitleBottom };
            blend.Positions = new[] { 0f, 0.5f, 1f };
            var brush = new LinearGradientBrush(rectangle2, Color.Black, Color.Black, 90f)
            {
                InterpolationColors = blend
            };
            graphics.FillRectangle(brush, rectangle2);
            graphics.DrawRectangle(Border, rectangle2);
            tabRect = new Rectangle(rectangle2.Location.X + 1, rectangle2.Location.Y + 1, rectangle2.Width - 2,
                rectangle2.Height - 2);
            graphics.DrawRectangle(new Pen(Color.FromArgb(15, Color.Black)), tabRect);
            location = GetTabRect(index).Location;
            point = new Point(GetTabRect(index).Location.X - 2, location.Y - 2);
            itemSize = new Size(GetTabRect(index).Width + 3, GetTabRect(index).Height + 1);
            rectangle2 = new Rectangle(point, itemSize);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            var pointArray = new Point[3];
            itemSize = ItemSize;
            tabRect = GetTabRect(index);
            point3 = tabRect.Location;
            location = new Point(itemSize.Height - 3, point3.Y + 20);
            pointArray[0] = location;
            point = GetTabRect(index).Location;
            point4 = new Point(ItemSize.Height + 4, point.Y + 14);
            pointArray[1] = point4;
            size3 = ItemSize;
            point5 = new Point(size3.Height + 4, GetTabRect(index).Location.Y + 0x1b);
            pointArray[2] = point5;

            if (ImageList != null)
            {
                try
                {
                    if (ImageList.Images[TabPages[index].ImageIndex] != null)
                    {
                        point5 = rectangle2.Location;
                        point4 = new Point(point5.X + 8, rectangle2.Location.Y + 6);
                        graphics.DrawImage(ImageList.Images[TabPages[index].ImageIndex], point4);
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString("      " + TabPages[index].Text, Font, Brushes.Black, rectangle2, format);
                    }
                    else
                    {
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString(TabPages[index].Text, Font, Brushes.Black, rectangle2, format);
                    }
                    goto Label_09D5;
                }
                catch
                {
                    format = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };
                    graphics.DrawString(TabPages[index].Text, Font, Brushes.Black, rectangle2, format);
                    goto Label_09D5;
                }
            }
            format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            graphics.DrawString(TabPages[index].Text, Font, Brushes.Black, rectangle2, format);
        }
        else
        {
            tabRect = GetTabRect(index);
            point5 = tabRect.Location;
            Point point6 = GetTabRect(index).Location;
            point4 = new Point(point5.X - 1, point6.Y - 1);
            size3 = new Size(GetTabRect(index).Width + 2, GetTabRect(index).Height);
            var layoutRectangle = new Rectangle(point4, size3);
            point5 = new Point(layoutRectangle.Right, layoutRectangle.Top);
            point6 = new Point(layoutRectangle.Right, layoutRectangle.Bottom);
            graphics.DrawLine(Border, point5, point6);
            point5 = new Point(layoutRectangle.Right - 1, layoutRectangle.Top);
            point6 = new Point(layoutRectangle.Right - 1, layoutRectangle.Bottom);
            graphics.DrawLine(new Pen(TitleTop), point5, point6);
            if (ImageList != null)
            {
                try
                {
                    if (ImageList.Images[TabPages[index].ImageIndex] != null)
                    {
                        point5 = layoutRectangle.Location;
                        point4 = new Point(point5.X + 8, layoutRectangle.Location.Y + 6);
                        graphics.DrawImage(ImageList.Images[TabPages[index].ImageIndex], point4);
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString("      " + TabPages[index].Text, Font,
                            new SolidBrush(Color.FromArgb(170, 170, 170)), layoutRectangle, format);
                    }
                    else
                    {
                        format = new StringFormat
                        {
                            LineAlignment = StringAlignment.Center,
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString(TabPages[index].Text, Font, new SolidBrush(Color.FromArgb(170, 170, 170)),
                            layoutRectangle, format);
                    }
                    goto Label_09D5;
                }
                catch
                {
                    format = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Center
                    };
                    graphics.DrawString(TabPages[index].Text, Font, new SolidBrush(Color.FromArgb(170, 170, 170)),
                        layoutRectangle, format);
                    goto Label_09D5;
                }
            }
            format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            graphics.DrawString(TabPages[index].Text, Font, new SolidBrush(Color.FromArgb(170, 170, 170)),
                layoutRectangle, format);
        }
    Label_09D5:
        index++;
        goto Label_0147;
    }

    /// <summary>
    ///     Raises the <see cref="Control.Paint" /> event.
    /// </summary>
    /// <param name="e">
    ///     A <see cref="System.Windows.Forms.PaintEventArgs" /> that contains the event
    ///     data.
    /// </param>
    /// <summary>
    ///     Raises the <see cref="TabControl.Selecting" /> event.
    /// </summary>
    /// <param name="e">
    ///     A <see cref="TabControlCancelEventArgs" /> that contains the event data.
    /// </param>
    protected override void OnSelecting(TabControlCancelEventArgs e)
    {
        try
        {
            if (TransitionEnabled)
            {
                if (_oldIndex < e.TabPageIndex)
                {
                    TransitionRight(TabPages[_oldIndex], TabPages[e.TabPageIndex]);
                }
                else
                {
                    TransitionLeft(TabPages[_oldIndex], TabPages[e.TabPageIndex]);
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    ///     Raises the <see cref="TabControl.Deselecting" /> event.
    /// </summary>
    /// <param name="e">
    ///     A <see cref="TabControlCancelEventArgs" /> that contains the event data
    /// </param>
    protected override void OnDeselecting(TabControlCancelEventArgs e)
    {
        _oldIndex = e.TabPageIndex;
    }

    /// <summary>
    ///     Slides right from one <see cref="TabPage" /> to another.
    /// </summary>
    /// <param name="firstTabPage">The <see cref="TabPage" /> to slide from. </param>
    /// <param name="secondTabPage">The <see cref="TabPage" /> to slide to.</param>
    private void TransitionRight(TabPage firstTabPage, TabPage secondTabPage)
    {
        Graphics graphics = firstTabPage.CreateGraphics();

        var firstTabBitmap = new Bitmap(firstTabPage.Width, firstTabPage.Height);
        var secondTabBitmap = new Bitmap(secondTabPage.Width, secondTabPage.Height);

        firstTabPage.DrawToBitmap(firstTabBitmap,
            new Rectangle(Point.Empty, new Size(firstTabPage.Width, firstTabPage.Height)));
        secondTabPage.DrawToBitmap(secondTabBitmap,
            new Rectangle(Point.Empty, new Size(secondTabPage.Width, secondTabPage.Height)));

        foreach (Control control in firstTabPage.Controls)
            control.Hide();

        int slide = firstTabPage.Width - (firstTabPage.Width % _transitionSpeed);

        int a;
        for (a = 0; a >= -slide; a += -_transitionSpeed)
        {
            graphics.DrawImage(firstTabBitmap,
                new Rectangle(new Point(a, 0), new Size(firstTabPage.Width, firstTabPage.Height)));
            graphics.DrawImage(secondTabBitmap,
                new Rectangle(new Point(a + secondTabPage.Width, 0), new Size(secondTabPage.Width, secondTabPage.Height)));
        }
        a = firstTabPage.Width;

        graphics.DrawImage(firstTabBitmap,
            new Rectangle(new Point(a, 0), new Size(firstTabPage.Width, firstTabPage.Height)));
        graphics.DrawImage(secondTabBitmap,
            new Rectangle(new Point(a + secondTabPage.Width, 0), new Size(secondTabPage.Width, secondTabPage.Height)));

        SelectedTab = secondTabPage;

        foreach (Control control in secondTabPage.Controls)
            control.Show();

        foreach (Control control in firstTabPage.Controls)
            control.Show();

        graphics.Dispose();
        firstTabBitmap.Dispose();
        secondTabBitmap.Dispose();
    }

    /// <summary>
    ///     Slides left from one <see cref="TabPage" /> to another.
    /// </summary>
    /// <param name="firstTabPage">The <see cref="TabPage" /> to slide from. </param>
    /// <param name="secondTabPage">The <see cref="TabPage" /> to slide to.</param>
    private void TransitionLeft(TabPage firstTabPage, TabPage secondTabPage)
    {
        Graphics graphics = firstTabPage.CreateGraphics();

        var firstTabBitmap = new Bitmap(firstTabPage.Width, firstTabPage.Height);
        var secondTabBitmap = new Bitmap(secondTabPage.Width, secondTabPage.Height);

        firstTabPage.DrawToBitmap(firstTabBitmap,
            new Rectangle(Point.Empty, new Size(firstTabPage.Width, firstTabPage.Height)));
        secondTabPage.DrawToBitmap(secondTabBitmap,
            new Rectangle(Point.Empty, new Size(secondTabPage.Width, secondTabPage.Height)));

        foreach (Control control in firstTabPage.Controls)
            control.Hide();

        int slide = firstTabPage.Width - (firstTabPage.Width % _transitionSpeed);

        int a;
        for (a = 0; a <= slide; a += _transitionSpeed)
        {
            graphics.DrawImage(firstTabBitmap,
                new Rectangle(new Point(a, 0), new Size(firstTabPage.Width, firstTabPage.Height)));
            graphics.DrawImage(secondTabBitmap,
                new Rectangle(new Point(a - secondTabPage.Width, 0), new Size(secondTabPage.Width, secondTabPage.Height)));
        }
        a = firstTabPage.Width;

        graphics.DrawImage(firstTabBitmap,
            new Rectangle(new Point(a, 0), new Size(firstTabPage.Width, firstTabPage.Height)));
        graphics.DrawImage(secondTabBitmap,
            new Rectangle(new Point(a - secondTabPage.Width, 0), new Size(secondTabPage.Width, secondTabPage.Height)));

        SelectedTab = secondTabPage;

        foreach (Control control in secondTabPage.Controls)
            control.Show();

        foreach (Control control in firstTabPage.Controls)
            control.Show();

        graphics.Dispose();
        firstTabBitmap.Dispose();
        secondTabBitmap.Dispose();
    }
}

[DefaultEvent("CheckedChanged")]
internal class sexyWhite_Check : ThemeControl154
{
    private readonly Color Accent = Color.FromArgb(135, 177, 74);
    private Color Border = Color.DarkGray;
    private readonly Color TextColor = Color.FromArgb(23, 23, 23);
    private readonly Color TitleBottom = Color.FromArgb(170, 170, 170);
    private readonly Color TitleTop = Color.FromArgb(245, 245, 245);
    private Color Basecolor = Color.FromArgb(232, 232, 232);
    // Fields
    public delegate void CheckedChangedEventHandler(object sender);


    private Color C1;
    private Color C2;
    private CheckedChangedEventHandler CheckedChangedEvent;
    private Color Glow;
    private Color TC;
    private Color UC1;
    private Color UC2;
    private int X;
    private bool _Checked;

    // Events

    // Methods
    public sexyWhite_Check()
    {
        LockHeight = 0x10;
        SetColor("Border", Border);
        SetColor("Checked1", Color.ForestGreen);
        SetColor("Checked2", Accent);
        SetColor("Unchecked1", TitleTop);
        SetColor("Unchecked2", TitleBottom);
        SetColor("Glow", 15, Accent);
        SetColor("Text", TextColor);
    }

    public bool Checked
    {
        get { return _Checked; }
        set
        {
            _Checked = value;
            Invalidate();
        }
    }

    public event CheckedChangedEventHandler CheckedChanged;


    protected override void ColorHook()
    {
        C1 = GetColor("Checked1");
        C2 = GetColor("Checked2");
        UC1 = GetColor("Unchecked1");
        UC2 = GetColor("Unchecked2");
        Border = GetColor("Border");
        Glow = GetColor("Glow");
        TC = GetColor("Text");
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        _Checked = !_Checked;
        CheckedChangedEventHandler checkedChangedEvent = CheckedChangedEvent;
        if (checkedChangedEvent != null)
        {
            checkedChangedEvent(this);
        }
        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        X = e.Location.X;
        Invalidate();
    }

    protected override void PaintHook()
    {
        base.G.Clear(BackColor);
        if (_Checked)
        {
            DrawGradient(C1, C2, 1, 1, 14, 14);
            var point = new Point(-3, -1);
            base.G.DrawString("a", new Font("Marlett", 13f), Brushes.DimGray, point);
        }
        else
        {
            DrawGradient(UC1, UC2, 1, 1, 14, 14, 90f);
        }
        if ((base.State == MouseState.Over) & (X < 0x10))
        {
            if (_Checked)
            {
                base.G.FillRectangle(new SolidBrush(Glow), 1, 1, 14, 14);
            }
            else
            {
                base.G.FillRectangle(new SolidBrush(Color.FromArgb(10, Glow)), 1, 1, 14, 14);
            }
        }
        DrawBorders(new Pen(Border), 0, 0, 0x10, 0x10, 1);
        DrawText(new SolidBrush(TC), HorizontalAlignment.Left, 20, 0);
    }

    // Properties
}


internal class SexySwitch : Control
{
    #region Properties

    private bool _Switched;

    public bool Switched
    {
        get { return _Switched; }
        set
        {
            _Switched = value;
            Invalidate();
        }
    }

    private Color _BackColorTop,
        _BackColorBottom,
        _OutlineColor,
        _TextColor,
        _TextBackColor,
        _DisabledTopColor,
        _DisabledBottomColor,
        _ActivatedColor,
        _ActivatedBackColor,
        _DeactivatedColor,
        _DeactivatedBackColor;

    public virtual Color ActivatedColor
    {
        get { return _ActivatedColor; }
        set
        {
            _ActivatedColor = value;
            Invalidate();
        }
    }

    public virtual Color ActivatedBackColor
    {
        get { return _ActivatedBackColor; }
        set
        {
            _ActivatedBackColor = value;
            Invalidate();
        }
    }

    public virtual Color DeactivatedColor
    {
        get { return _DeactivatedColor; }
        set
        {
            _DeactivatedColor = value;
            Invalidate();
        }
    }

    public virtual Color DeactivatedBackColor
    {
        get { return _DeactivatedBackColor; }
        set
        {
            _DeactivatedBackColor = value;
            Invalidate();
        }
    }

    public virtual Color DisabledTopColor
    {
        get { return _DisabledTopColor; }
        set
        {
            _DisabledTopColor = value;
            Invalidate();
        }
    }

    public virtual Color DisabledBottomColor
    {
        get { return _DisabledBottomColor; }
        set
        {
            _DisabledBottomColor = value;
            Invalidate();
        }
    }

    public virtual Color TextColor
    {
        get { return _TextColor; }
        set
        {
            _TextColor = value;
            Invalidate();
        }
    }

    public virtual Color TextBackColor
    {
        get { return _TextBackColor; }
        set
        {
            _TextBackColor = value;
            Invalidate();
        }
    }

    public virtual Color OutlineColor
    {
        get { return _OutlineColor; }
        set
        {
            _OutlineColor = value;
            Invalidate();
        }
    }

    public virtual Color BackColorTop
    {
        get { return _BackColorTop; }
        set
        {
            _BackColorTop = value;
            Invalidate();
        }
    }

    public virtual Color BackColorBottom
    {
        get { return _BackColorBottom; }
        set
        {
            _BackColorBottom = value;
            Invalidate();
        }
    }

    private int _Radius = 3, _OutlineThickness = 1;

    public virtual int Radius
    {
        get { return _Radius; }
        set
        {
            _Radius = value;
            Invalidate();
        }
    }

    public virtual int OutlineThickness
    {
        get { return _OutlineThickness; }
        set
        {
            _OutlineThickness = value;
            Invalidate();
        }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override RightToLeft RightToLeft
    {
        get { return base.RightToLeft; }
        set { base.RightToLeft = value; }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool UseWaitCursor
    {
        get { return base.UseWaitCursor; }
        set { base.UseWaitCursor = value; }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new int TabIndex
    {
        get { return base.TabIndex; }
        set { base.TabIndex = value; }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool TabStop
    {
        get { return base.TabStop; }
        set { base.TabStop = value; }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MinimumSize
    {
        get { return base.MinimumSize; }
        set { base.MinimumSize = value; }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Size MaximumSize
    {
        get { return base.MaximumSize; }
        set { base.MaximumSize = value; }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new ImeMode ImeMode
    {
        get { return base.ImeMode; }
        set { base.ImeMode = value; }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override Image BackgroundImage
    {
        get { return base.BackgroundImage; }
        set { base.BackgroundImage = value; }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ImageLayout BackgroundImageLayout
    {
        get { return base.BackgroundImageLayout; }
        set { base.BackgroundImageLayout = value; }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override DockStyle Dock
    {
        get { return base.Dock; }
        set { base.Dock = value; }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public new bool CausesValidation
    {
        get { return base.CausesValidation; }
        set { base.CausesValidation = value; }
    }

    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(false)]
    public override ContextMenuStrip ContextMenuStrip
    {
        get { return base.ContextMenuStrip; }
        set { base.ContextMenuStrip = value; }
    }

    #endregion

    public SexySwitch()
    {
        Cursor = Cursors.Hand;
        Font = new Font("Verdana", 6, FontStyle.Regular);
        OutlineColor = Color.FromArgb(30, 30, 30);
        BackColorTop = Color.FromArgb(60, 60, 60);
        BackColorBottom = Color.FromArgb(40, 40, 40);
        ActivatedColor = Color.LimeGreen;
        ActivatedBackColor = Color.DarkGreen;
        DeactivatedColor = Color.Red;
        DeactivatedBackColor = Color.DarkRed;
        TextColor = Color.White;
        TextBackColor = Color.Black;
        MinimumSize = new Size(40, 15);
        Size = MinimumSize;
        MaximumSize = Size;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent.BackColor);
        SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 2, Width, (Height - 4)), Radius,
            new Pen(OutlineColor, OutlineThickness),
            SharpTwist.CreateGradient(Width, Height, BackColorTop, BackColorBottom));
        if (Enabled)
        {
            if (Switched)
            {
                SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle((Width / 2), 0, (Width / 2), Height), Radius,
                    new Pen(OutlineColor, OutlineThickness),
                    SharpTwist.CreateGradient(Width, Height, ActivatedColor, ActivatedBackColor));
                SharpTwist.DrawString(e.Graphics, Font, "Ein", TextColor, StringAlignment.Near, new PointF(2, 3));
            }
            else
            {
                SharpTwist.DrawRoundRectangle(e.Graphics, new Rectangle(0, 0, (Width / 2), Height), Radius,
                    new Pen(OutlineColor, OutlineThickness),
                    SharpTwist.CreateGradient(Width, Height, DeactivatedColor, DeactivatedBackColor));
                SharpTwist.DrawString(e.Graphics, Font, "Aus", TextColor, StringAlignment.Near,
                    new PointF(((Width / 2) + 2), 3));
            }
        }
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) Switched = !Switched;
        base.OnMouseClick(e);
    }
}


internal class sexyButton : Control
{
    private const byte Count = 7;
    private Bitmap B;
    private SolidBrush B1;
    private SolidBrush B2;
    private LinearGradientBrush B3;
    private Pigment[] C;
    private bool Down;
    private Graphics G;
    private Pen P1;
    private Pen P2;
    private Point PT;
    private Size SZ;
    private bool Shadow_ = true;

    public sexyButton()
    {
        SetStyle((ControlStyles)8198, true);
        Colors = new[]
        {
            new Pigment("Border", Color.FromArgb(150, 255, 0)),
            new Pigment("Backcolor", Color.FromArgb(247, 247, 251)),
            new Pigment("Highlight", Color.FromArgb(112, 250, 112)),
            new Pigment("Gradient1", Color.FromArgb(150, 255, 150)),
            new Pigment("Gradient2", Color.FromArgb(140, 255, 1)),
            new Pigment("Text Color", Color.DarkGray),
            new Pigment("Text Shadow", Color.FromArgb(30, 0, 0, 0))
        };
        Font = new Font("Verdana", 8);
    }

    public bool Shadow
    {
        get { return Shadow_; }
        set
        {
            Shadow_ = value;
            Invalidate();
        }
    }

    public Pigment[] Colors
    {
        get { return C; }
        set
        {
            if (value.Length != Count)
                throw new IndexOutOfRangeException();

            P1 = new Pen(value[0].Value);
            P2 = new Pen(value[2].Value);

            B1 = new SolidBrush(value[6].Value);
            B2 = new SolidBrush(value[5].Value);

            C = value;
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        B = new Bitmap(Width, Height);
        G = Graphics.FromImage(B);

        if (Down)
        {
            B3 = new LinearGradientBrush(ClientRectangle, C[4].Value, C[3].Value, 90f);
        }
        else
        {
            B3 = new LinearGradientBrush(ClientRectangle, C[3].Value, C[4].Value, 90f);
        }
        G.FillRectangle(B3, ClientRectangle);

        if (!string.IsNullOrEmpty(Text))
        {
            SZ = G.MeasureString(Text, Font).ToSize();
            PT = new Point(Convert.ToInt32(Width / 2 - SZ.Width / 2), Convert.ToInt32(Height / 2 - SZ.Height / 2));
            if (Shadow_)
                G.DrawString(Text, Font, B1, PT.X + 1, PT.Y + 1);
            G.DrawString(Text, Font, B2, PT);
        }

        G.DrawRectangle(P1, new Rectangle(0, 0, Width - 1, Height - 1));
        G.DrawRectangle(P2, new Rectangle(1, 1, Width - 3, Height - 3));

        B.SetPixel(0, 0, C[1].Value);
        B.SetPixel(Width - 1, 0, C[1].Value);
        B.SetPixel(Width - 1, Height - 1, C[1].Value);
        B.SetPixel(0, Height - 1, C[1].Value);

        e.Graphics.DrawImage(B, 0, 0);
        B3.Dispose();
        G.Dispose();
        B.Dispose();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            Down = true;
            Invalidate();
        }
        base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        Down = false;
        Invalidate();
        base.OnMouseUp(e);
    }
}
