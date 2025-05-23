// Copyright Dirk Lemstra https://github.com/dlemstra/Magick.NET.
// Licensed under the Apache License, Version 2.0.

#if Q8
using QuantumType = System.Byte;
#elif Q16
using QuantumType = System.UInt16;
#elif Q16HDRI
using QuantumType = System.Single;
#else
#error Not implemented!
#endif

namespace ImageMagick.Drawing;

/// <summary>
/// Specifies the color of a background rectangle to place under text annotations.
/// </summary>
public sealed partial class DrawableTextUnderColor : IDrawableTextUnderColor<QuantumType>, IDrawingWand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DrawableTextUnderColor"/> class.
    /// </summary>
    /// <param name="color">The color to use.</param>
    public DrawableTextUnderColor(IMagickColor<QuantumType> color)
    {
        Throw.IfNull(color);

        Color = color;
    }

    /// <summary>
    /// Gets the color to use.
    /// </summary>
    public IMagickColor<QuantumType> Color { get; }

    /// <summary>
    /// Draws this instance with the drawing wand.
    /// </summary>
    /// <param name="wand">The want to draw on.</param>
    void IDrawingWand.Draw(DrawingWand wand)
        => wand?.TextUnderColor(Color);
}
