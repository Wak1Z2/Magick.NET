﻿// Copyright Dirk Lemstra https://github.com/dlemstra/Magick.NET.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Globalization;
using ImageMagick.Drawing;

#if Q8
using QuantumType = System.Byte;
#elif Q16
using QuantumType = System.UInt16;
#elif Q16HDRI
using QuantumType = System.Single;
#else
#error Not implemented!
#endif

namespace ImageMagick;

/// <content />
public partial class MagickImage
{
    private class CloneMutator : IMagickImageCloneMutator<QuantumType>, IDisposable
    {
        private IntPtr _result = IntPtr.Zero;

        public CloneMutator(NativeMagickImage nativeMagickImage)
            => NativeMagickImage = nativeMagickImage;

        protected NativeMagickImage NativeMagickImage { get; }

        public void Dispose()
        {
            if (_result != IntPtr.Zero)
                NativeMagickImage.DisposeInstance(_result);
        }

        public IntPtr GetResult()
        {
            var result = _result;
            _result = IntPtr.Zero;
            return result;
        }

        public void AdaptiveBlur()
            => AdaptiveBlur(0.0, 1.0);

        public void AdaptiveBlur(double radius)
            => AdaptiveBlur(radius, 1.0);

        public void AdaptiveBlur(double radius, double sigma)
            => SetResult(NativeMagickImage.AdaptiveBlur(radius, sigma));

        public void AdaptiveResize(uint width, uint height)
            => AdaptiveResize(new MagickGeometry(width, height));

        public void AdaptiveResize(IMagickGeometry geometry)
        {
            Throw.IfNull(geometry);

            SetResult(NativeMagickImage.AdaptiveResize(geometry.ToString()));
        }

        public void AdaptiveSharpen()
            => AdaptiveSharpen(0.0, 1.0);

        public void AdaptiveSharpen(Channels channels)
            => AdaptiveSharpen(0.0, 1.0, channels);

        public void AdaptiveSharpen(double radius, double sigma)
            => AdaptiveSharpen(radius, sigma, ImageMagick.Channels.Undefined);

        public void AdaptiveSharpen(double radius, double sigma, Channels channels)
            => SetResult(NativeMagickImage.AdaptiveSharpen(radius, sigma, channels));

        public void AdaptiveThreshold(uint width, uint height)
            => AdaptiveThreshold(width, height, 0.0, ImageMagick.Channels.Undefined);

        public void AdaptiveThreshold(uint width, uint height, Channels channels)
            => AdaptiveThreshold(width, height, 0.0, channels);

        public void AdaptiveThreshold(uint width, uint height, double bias)
            => AdaptiveThreshold(width, height, bias, ImageMagick.Channels.Undefined);

        public void AdaptiveThreshold(uint width, uint height, double bias, Channels channels)
            => SetResult(NativeMagickImage.AdaptiveThreshold(width, height, bias, channels));

        public void AdaptiveThreshold(uint width, uint height, Percentage biasPercentage)
            => AdaptiveThreshold(width, height, biasPercentage, ImageMagick.Channels.Undefined);

        public void AdaptiveThreshold(uint width, uint height, Percentage biasPercentage, Channels channels)
            => AdaptiveThreshold(width, height, PercentageHelper.ToQuantum(nameof(biasPercentage), biasPercentage), channels);

        public void AddNoise(NoiseType noiseType)
            => AddNoise(noiseType, ImageMagick.Channels.Undefined);

        public void AddNoise(NoiseType noiseType, Channels channels)
            => AddNoise(noiseType, 1.0, channels);

        public void AddNoise(NoiseType noiseType, double attenuate)
            => AddNoise(noiseType, attenuate, ImageMagick.Channels.Undefined);

        public void AddNoise(NoiseType noiseType, double attenuate, Channels channels)
            => SetResult(NativeMagickImage.AddNoise(noiseType, attenuate, channels));

        public void AffineTransform(IDrawableAffine affineMatrix)
        {
            Throw.IfNull(affineMatrix);

            SetResult(NativeMagickImage.AffineTransform(affineMatrix.ScaleX, affineMatrix.ScaleY, affineMatrix.ShearX, affineMatrix.ShearY, affineMatrix.TranslateX, affineMatrix.TranslateY));
        }

        public void AutoOrient()
            => SetResult(NativeMagickImage.AutoOrient());

        public void BilateralBlur(uint width, uint height)
        {
            var intensitySigma = Math.Sqrt((width * width) + (height * height));
            BilateralBlur(width, height, intensitySigma, intensitySigma * 0.25);
        }

        public void BilateralBlur(uint width, uint height, double intensitySigma, double spatialSigma)
            => SetResult(NativeMagickImage.BilateralBlur(width, height, intensitySigma, spatialSigma));

        public void BlueShift()
            => BlueShift(1.5);

        public void BlueShift(double factor)
            => SetResult(NativeMagickImage.BlueShift(factor));

        public void Blur()
            => Blur(0.0, 1.0);

        public void Blur(Channels channels)
            => Blur(0.0, 1.0, channels);

        public void Blur(double radius, double sigma)
            => Blur(radius, sigma, ImageMagick.Channels.Undefined);

        public void Blur(double radius, double sigma, Channels channels)
            => SetResult(NativeMagickImage.Blur(radius, sigma, channels));

        public void Border(uint size)
            => Border(size, size);

        public void Border(uint width, uint height)
        {
            var rectangle = new MagickRectangle(0, 0, width, height);
            SetResult(NativeMagickImage.Border(rectangle));
        }

        public void Border(Percentage percentage)
            => Border((uint)(NativeMagickImage.Width_Get() * percentage), (uint)(NativeMagickImage.Height_Get() * percentage));

        public void CannyEdge()
            => CannyEdge(0.0, 1.0, new Percentage(10), new Percentage(30));

        public void CannyEdge(double radius, double sigma, Percentage lower, Percentage upper)
            => SetResult(NativeMagickImage.CannyEdge(radius, sigma, lower.ToDouble() / 100, upper.ToDouble() / 100));

        public void Charcoal()
            => Charcoal(0.0, 1.0);

        public void Charcoal(double radius, double sigma)
            => SetResult(NativeMagickImage.Charcoal(radius, sigma));

        public void Chop(IMagickGeometry geometry)
            => SetResult(NativeMagickImage.Chop(MagickRectangle.FromGeometry(geometry, (uint)NativeMagickImage.Width_Get(), (uint)NativeMagickImage.Height_Get())));

        public void ChopHorizontal(int x, uint width)
             => Chop(new MagickGeometry(x, 0, width, 0));

        public void ChopVertical(int y, uint height)
            => Chop(new MagickGeometry(0, y, 0, height));

        public void Colorize(IMagickColor<QuantumType> color, Percentage alpha)
        {
            Throw.IfNegative(alpha);

            Colorize(color, alpha, alpha, alpha);
        }

        public void Colorize(IMagickColor<QuantumType> color, Percentage alphaRed, Percentage alphaGreen, Percentage alphaBlue)
        {
            Throw.IfNull(color);
            Throw.IfNegative(alphaRed);
            Throw.IfNegative(alphaGreen);
            Throw.IfNegative(alphaBlue);

            var blend = string.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}", alphaRed.ToInt32(), alphaGreen.ToInt32(), alphaBlue.ToInt32());

            SetResult(NativeMagickImage.Colorize(color, blend));
        }

        public void ColorMatrix(IMagickColorMatrix matrix)
        {
            Throw.IfNull(matrix);

            SetResult(NativeMagickImage.ColorMatrix(matrix));
        }

        public void Convolve(IConvolveMatrix matrix)
        {
            Throw.IfNull(matrix);

            SetResult(NativeMagickImage.Convolve(matrix));
        }

        public void Crop(uint width, uint height)
            => Crop(width, height, Gravity.Undefined);

        public void Crop(uint width, uint height, Gravity gravity)
            => Crop(new MagickGeometry(0, 0, width, height), gravity);

        public void Crop(IMagickGeometry geometry)
            => Crop(geometry, Gravity.Undefined);

        public void Crop(IMagickGeometry geometry, Gravity gravity)
        {
            Throw.IfNull(geometry);

            SetResult(NativeMagickImage.Crop(geometry.ToString(), gravity));
        }

        public double Deskew(Percentage threshold)
            => Deskew(threshold, autoCrop: false);

        public double DeskewAndCrop(Percentage threshold)
            => Deskew(threshold, autoCrop: true);

        public void Despeckle()
            => SetResult(NativeMagickImage.Despeckle());

        public void Distort(DistortMethod method, params double[] arguments)
            => Distort(new DistortSettings(method), arguments);

        public void Distort(IDistortSettings settings, params double[] arguments)
        {
            Throw.IfNull(settings);
            Throw.IfNullOrEmpty(arguments);

            using var temporaryDefines = new TemporaryDefines(NativeMagickImage);
            temporaryDefines.SetArtifact("distort:scale", settings.Scale);
            temporaryDefines.SetArtifact("distort:viewport", settings.Viewport);

            SetResult(NativeMagickImage.Distort(settings.Method, settings.Bestfit, arguments, (nuint)arguments.Length));
        }

        public void Edge(double radius)
            => SetResult(NativeMagickImage.Edge(radius));

        public void Emboss()
            => Emboss(0.0, 1.0);

        public void Emboss(double radius, double sigma)
            => SetResult(NativeMagickImage.Emboss(radius, sigma));

        public void Enhance()
            => SetResult(NativeMagickImage.Enhance());

        public void Extent(uint width, uint height)
           => Extent(new MagickGeometry(width, height));

        public void Extent(int x, int y, uint width, uint height)
            => Extent(new MagickGeometry(x, y, width, height));

        public void Extent(uint width, uint height, IMagickColor<QuantumType> backgroundColor)
            => Extent(new MagickGeometry(width, height), backgroundColor);

        public void Extent(uint width, uint height, Gravity gravity)
            => Extent(new MagickGeometry(width, height), gravity);

        public void Extent(uint width, uint height, Gravity gravity, IMagickColor<QuantumType> backgroundColor)
            => Extent(new MagickGeometry(width, height), gravity, backgroundColor);

        public void Extent(IMagickGeometry geometry)
            => Extent(geometry, Gravity.Undefined);

        public void Extent(IMagickGeometry geometry, IMagickColor<QuantumType> backgroundColor)
        {
            Throw.IfNull(backgroundColor);

            NativeMagickImage.BackgroundColor_Set(backgroundColor);
            Extent(geometry);
        }

        public void Extent(IMagickGeometry geometry, Gravity gravity)
        {
            Throw.IfNull(geometry);

            SetResult(NativeMagickImage.Extent(geometry.ToString(), gravity));
        }

        public void Extent(IMagickGeometry geometry, Gravity gravity, IMagickColor<QuantumType> backgroundColor)
        {
            Throw.IfNull(backgroundColor);

            NativeMagickImage.BackgroundColor_Set(backgroundColor);
            Extent(geometry, gravity);
        }

        public void Flip()
            => SetResult(NativeMagickImage.Flip());

        public void Flop()
            => SetResult(NativeMagickImage.Flop());

        public void Frame()
            => Frame(new MagickGeometry(6, 6, 25, 25));

        public void Frame(IMagickGeometry geometry)
            => SetResult(NativeMagickImage.Frame(MagickRectangle.FromGeometry(geometry, (uint)NativeMagickImage.Width_Get(), (uint)NativeMagickImage.Height_Get())));

        public void Frame(uint width, uint height)
            => Frame(new MagickGeometry(6, 6, width, height));

        public void Frame(uint width, uint height, int innerBevel, int outerBevel)
            => Frame(new MagickGeometry(innerBevel, outerBevel, width, height));

        public void GaussianBlur(double radius)
            => GaussianBlur(radius, 1.0);

        public void GaussianBlur(double radius, Channels channels)
            => GaussianBlur(radius, 1.0, channels);

        public void GaussianBlur(double radius, double sigma)
            => GaussianBlur(radius, sigma, ImageMagick.Channels.Undefined);

        public void GaussianBlur(double radius, double sigma, Channels channels)
            => SetResult(NativeMagickImage.GaussianBlur(radius, sigma, channels));

        public void HoughLine()
            => HoughLine(0, 0, 40);

        public void HoughLine(uint width, uint height, uint threshold)
            => SetResult(NativeMagickImage.HoughLine(width, height, threshold));

        public void Implode(double amount, PixelInterpolateMethod method)
            => SetResult(NativeMagickImage.Implode(amount, method));

        public void InterpolativeResize(uint width, uint height, PixelInterpolateMethod method)
            => InterpolativeResize(new MagickGeometry(width, height), method);

        public void InterpolativeResize(IMagickGeometry geometry, PixelInterpolateMethod method)
        {
            Throw.IfNull(geometry);

            SetResult(NativeMagickImage.InterpolativeResize(geometry.ToString(), method));
        }

        public void InterpolativeResize(Percentage percentage, PixelInterpolateMethod method)
            => InterpolativeResize(new MagickGeometry(percentage, percentage), method);

        public void InterpolativeResize(Percentage percentageWidth, Percentage percentageHeight, PixelInterpolateMethod method)
            => InterpolativeResize(new MagickGeometry(percentageWidth, percentageHeight), method);

        public void Kuwahara()
            => Kuwahara(0.0, 1.0);

        public void Kuwahara(double radius, double sigma)
            => SetResult(NativeMagickImage.Kuwahara(radius, sigma));

        public void LiquidRescale(uint width, uint height)
        => LiquidRescale(new MagickGeometry(width, height));

        public void LiquidRescale(uint width, uint height, double deltaX, double rigidity)
        {
            var geometry = new MagickGeometry(width, height);

            SetResult(NativeMagickImage.LiquidRescale(geometry.ToString(), deltaX, rigidity));
        }

        public void LiquidRescale(IMagickGeometry geometry)
        {
            Throw.IfNull(geometry);

            SetResult(NativeMagickImage.LiquidRescale(geometry.ToString(), geometry.X, geometry.Y));
        }

        public void LiquidRescale(Percentage percentage)
            => LiquidRescale(new MagickGeometry(percentage, percentage));

        public void LiquidRescale(Percentage percentageWidth, Percentage percentageHeight)
            => LiquidRescale(new MagickGeometry(percentageWidth, percentageHeight));

        public void LiquidRescale(Percentage percentageWidth, Percentage percentageHeight, double deltaX, double rigidity)
        {
            var geometry = new MagickGeometry(percentageWidth, percentageHeight);

            SetResult(NativeMagickImage.LiquidRescale(geometry.ToString(), deltaX, rigidity));
        }

        public void Magnify()
            => SetResult(NativeMagickImage.Magnify());

        public void MeanShift(uint size)
            => MeanShift(size, size);

        public void MeanShift(uint size, Percentage colorDistance)
            => MeanShift(size, size, colorDistance);

        public void MeanShift(uint width, uint height)
            => MeanShift(width, height, new Percentage(10));

        public void MeanShift(uint width, uint height, Percentage colorDistance)
            => SetResult(NativeMagickImage.MeanShift(width, height, PercentageHelper.ToQuantum(nameof(colorDistance), colorDistance)));

        public void Minify()
            => SetResult(NativeMagickImage.Minify());

        public void Morphology(IMorphologySettings settings)
        {
            Throw.IfNull(settings);
            Throw.IfTrue(settings.Iterations < -1, nameof(settings), "The number of iterations must be unlimited (-1) or positive");

            using var temporaryDefines = new TemporaryDefines(NativeMagickImage);
            temporaryDefines.SetArtifact("convolve:bias", settings.ConvolveBias);
            temporaryDefines.SetArtifact("convolve:scale", settings.ConvolveScale);

            if (settings.UserKernel is not null && settings.UserKernel.Length > 0)
            {
                SetResult(NativeMagickImage.Morphology(settings.Method, settings.UserKernel, settings.Channels, settings.Iterations));
            }
            else
            {
                var kernel = EnumHelper.GetName(settings.Kernel).ToLowerInvariant() + ":" + settings.KernelArguments;
                SetResult(NativeMagickImage.Morphology(settings.Method, kernel, settings.Channels, settings.Iterations));
            }
        }

        public void MotionBlur(double radius, double sigma, double angle)
            => SetResult(NativeMagickImage.MotionBlur(radius, sigma, angle));

        public void OilPaint()
            => OilPaint(3.0, 1.0);

        public void OilPaint(double radius, double sigma)
            => SetResult(NativeMagickImage.OilPaint(radius, sigma));

        public void Resample(double resolutionX, double resolutionY)
            => Resample(resolutionX, resolutionY, NativeMagickImage.FilterType_Get());

        public void Resample(double resolutionX, double resolutionY, FilterType filter)
            => SetResult(NativeMagickImage.Resample(resolutionX, resolutionY, filter));

        public void Resample(PointD density)
            => Resample(density, NativeMagickImage.FilterType_Get());

        public void Resample(PointD density, FilterType filter)
            => Resample(density.X, density.Y, filter);

        public void Resize(uint width, uint height)
            => Resize(width, height, NativeMagickImage.FilterType_Get());

        public void Resize(uint width, uint height, FilterType filter)
            => Resize(new MagickGeometry(width, height), filter);

        public void Resize(IMagickGeometry geometry)
            => Resize(geometry, NativeMagickImage.FilterType_Get());

        public void Resize(IMagickGeometry geometry, FilterType filter)
        {
            Throw.IfNull(geometry);

            SetResult(NativeMagickImage.Resize(geometry.ToString(), filter));
        }

        public void Resize(Percentage percentage)
            => Resize(percentage, NativeMagickImage.FilterType_Get());

        public void Resize(Percentage percentage, FilterType filter)
            => Resize(new MagickGeometry(percentage, percentage), filter);

        public void Resize(Percentage percentageWidth, Percentage percentageHeight)
            => Resize(percentageWidth, percentageHeight, NativeMagickImage.FilterType_Get());

        public void Resize(Percentage percentageWidth, Percentage percentageHeight, FilterType filter)
            => Resize(new MagickGeometry(percentageWidth, percentageHeight), filter);

        public void Roll(int x, int y)
            => SetResult(NativeMagickImage.Roll(x, y));

        public void Rotate(double degrees)
            => SetResult(NativeMagickImage.Rotate(degrees));

        public void RotationalBlur(double angle)
            => RotationalBlur(angle, ImageMagick.Channels.Undefined);

        public void RotationalBlur(double angle, Channels channels)
            => SetResult(NativeMagickImage.RotationalBlur(angle, channels));

        public void Sample(uint width, uint height)
            => Sample(new MagickGeometry(width, height));

        public void Sample(IMagickGeometry geometry)
        {
            Throw.IfNull(geometry);

            SetResult(NativeMagickImage.Sample(geometry.ToString()));
        }

        public void Sample(Percentage percentage)
            => Sample(percentage, percentage);

        public void Sample(Percentage percentageWidth, Percentage percentageHeight)
            => Sample(new MagickGeometry(percentageWidth, percentageHeight));

        public void Scale(uint width, uint height)
            => Scale(new MagickGeometry(width, height));

        public void Scale(IMagickGeometry geometry)
        {
            Throw.IfNull(geometry);

            SetResult(NativeMagickImage.Scale(geometry.ToString()));
        }

        public void Scale(Percentage percentage)
            => Scale(percentage, percentage);

        public void Scale(Percentage percentageWidth, Percentage percentageHeight)
            => Scale(new MagickGeometry(percentageWidth, percentageHeight));

        public void SelectiveBlur(double radius, double sigma, double threshold)
            => SelectiveBlur(radius, sigma, threshold, ImageMagick.Channels.Undefined);

        public void SelectiveBlur(double radius, double sigma, double threshold, Channels channels)
            => SetResult(NativeMagickImage.SelectiveBlur(radius, sigma, threshold, channels));

        public void SelectiveBlur(double radius, double sigma, Percentage thresholdPercentage)
            => SelectiveBlur(radius, sigma, thresholdPercentage, ImageMagick.Channels.Undefined);

        public void SelectiveBlur(double radius, double sigma, Percentage thresholdPercentage, Channels channels)
            => SetResult(NativeMagickImage.SelectiveBlur(radius, sigma, PercentageHelper.ToQuantum(nameof(thresholdPercentage), thresholdPercentage), channels));

        public void SepiaTone()
            => SepiaTone(new Percentage(80));

        public void SepiaTone(Percentage threshold)
            => SetResult(NativeMagickImage.SepiaTone(PercentageHelper.ToQuantum(nameof(threshold), threshold)));

        public void Shade()
            => Shade(30, 30);

        public void Shade(double azimuth, double elevation)
            => Shade(azimuth, elevation, ImageMagick.Channels.RGB);

        public void Shade(double azimuth, double elevation, Channels channels)
            => SetResult(NativeMagickImage.Shade(azimuth, elevation, false, channels));

        public void ShadeGrayscale()
            => ShadeGrayscale(30, 30);

        public void ShadeGrayscale(double azimuth, double elevation)
            => ShadeGrayscale(azimuth, elevation, ImageMagick.Channels.RGB);

        public void ShadeGrayscale(double azimuth, double elevation, Channels channels)
            => SetResult(NativeMagickImage.Shade(azimuth, elevation, true, channels));

        public void Shadow()
            => Shadow(5, 5, 0.5, new Percentage(80));

        public void Shadow(IMagickColor<QuantumType> color)
            => Shadow(5, 5, 0.5, new Percentage(80), color);

        public void Shadow(int x, int y, double sigma, Percentage alpha)
            => SetResult(NativeMagickImage.Shadow(x, y, sigma, alpha.ToDouble()));

        public void Shadow(int x, int y, double sigma, Percentage alpha, IMagickColor<QuantumType> color)
        {
            Throw.IfNull(color);

            var backgroundColor = NativeMagickImage.BackgroundColor_Get();
            NativeMagickImage.BackgroundColor_Set(color);
            try
            {
                SetResult(NativeMagickImage.Shadow(x, y, sigma, alpha.ToDouble()));
            }
            finally
            {
                NativeMagickImage.BackgroundColor_Set(backgroundColor);
            }
        }

        public void Sharpen()
            => Sharpen(0.0, 1.0);

        public void Sharpen(Channels channels)
            => Sharpen(0.0, 1.0, channels);

        public void Sharpen(double radius, double sigma)
            => Sharpen(radius, sigma, ImageMagick.Channels.Undefined);

        public void Sharpen(double radius, double sigma, Channels channels)
            => SetResult(NativeMagickImage.Sharpen(radius, sigma, channels));

        public void Shave(uint size)
            => Shave(size, size);

        public void Shave(uint leftRight, uint topBottom)
            => SetResult(NativeMagickImage.Shave(leftRight, topBottom));

        public void Shear(double xAngle, double yAngle)
            => SetResult(NativeMagickImage.Shear(xAngle, yAngle));

        public void Sketch()
            => Sketch(0.0, 1.0, 0.0);

        public void Sketch(double radius, double sigma, double angle)
            => SetResult(NativeMagickImage.Sketch(radius, sigma, angle));

        public void SparseColor(SparseColorMethod method, IEnumerable<ISparseColorArg<QuantumType>> args)
            => SparseColor(ImageMagick.Channels.Composite, method, args);

        public void SparseColor(SparseColorMethod method, params ISparseColorArg<QuantumType>[] args)
            => SparseColor(ImageMagick.Channels.Composite, method, (IEnumerable<ISparseColorArg<QuantumType>>)args);

        public void SparseColor(Channels channels, SparseColorMethod method, IEnumerable<ISparseColorArg<QuantumType>> args)
        {
            Throw.IfNull(args);

            var hasRed = EnumHelper.HasFlag(channels, ImageMagick.Channels.Red);
            var hasGreen = EnumHelper.HasFlag(channels, ImageMagick.Channels.Green);
            var hasBlue = EnumHelper.HasFlag(channels, ImageMagick.Channels.Blue);
            var hasAlpha = NativeMagickImage.HasAlpha_Get() && EnumHelper.HasFlag(channels, ImageMagick.Channels.Alpha);

            Throw.IfTrue(!hasRed && !hasGreen && !hasBlue && !hasAlpha, nameof(channels), "Invalid channels specified.");

            var arguments = new List<double>();

            var quantum = QuantumScaler.Create<QuantumType>();
            foreach (var arg in args)
            {
                arguments.Add(arg.X);
                arguments.Add(arg.Y);
                if (hasRed)
                    arguments.Add(quantum.ScaleToDouble(arg.Color.R));
                if (hasGreen)
                    arguments.Add(quantum.ScaleToDouble(arg.Color.G));
                if (hasBlue)
                    arguments.Add(quantum.ScaleToDouble(arg.Color.B));
                if (hasAlpha)
                    arguments.Add(quantum.ScaleToDouble(arg.Color.A));
            }

            Throw.IfTrue(arguments.Count == 0, nameof(args), "Value cannot be empty");

            SetResult(NativeMagickImage.SparseColor(channels, method, arguments.ToArray(), (nuint)arguments.Count));
        }

        public void SparseColor(Channels channels, SparseColorMethod method, params ISparseColorArg<QuantumType>[] args)
            => SparseColor(channels, method, (IEnumerable<ISparseColorArg<QuantumType>>)args);

        public void Splice(IMagickGeometry geometry)
            => SetResult(NativeMagickImage.Splice(MagickRectangle.FromGeometry(geometry, (uint)NativeMagickImage.Width_Get(), (uint)NativeMagickImage.Height_Get())));

        public void Spread()
            => Spread(NativeMagickImage.Interpolate_Get(), 3);

        public void Spread(double radius)
            => Spread(PixelInterpolateMethod.Undefined, radius);

        public void Spread(PixelInterpolateMethod method, double radius)
            => SetResult(NativeMagickImage.Spread(method, radius));

        public void Statistic(StatisticType type, uint width, uint height)
            => SetResult(NativeMagickImage.Statistic(type, width, height));

        public void Stegano(IMagickImage watermark)
        {
            Throw.IfNull(watermark);

            SetResult(NativeMagickImage.Stegano(watermark));
        }

        public void Stereo(IMagickImage rightImage)
        {
            Throw.IfNull(rightImage);

            SetResult(NativeMagickImage.Stereo(rightImage));
        }

        public void Swirl(double degrees)
            => Swirl(NativeMagickImage.Interpolate_Get(), degrees);

        public void Swirl(PixelInterpolateMethod method, double degrees)
            => SetResult(NativeMagickImage.Swirl(method, degrees));

        public void Thumbnail(uint width, uint height)
        => Thumbnail(new MagickGeometry(width, height));

        public void Thumbnail(IMagickGeometry geometry)
        {
            Throw.IfNull(geometry);

            SetResult(NativeMagickImage.Thumbnail(geometry.ToString()));
        }

        public void Thumbnail(Percentage percentage)
            => Thumbnail(percentage, percentage);

        public void Thumbnail(Percentage percentageWidth, Percentage percentageHeight)
            => Thumbnail(new MagickGeometry(percentageWidth, percentageHeight));

        public void Tint(IMagickGeometry opacity, IMagickColor<QuantumType> color)
        {
            Throw.IfNull(opacity);
            Throw.IfNull(color);

            SetResult(NativeMagickImage.Tint(opacity.ToString(), color));
        }

        public void Transpose()
            => SetResult(NativeMagickImage.Transpose());

        public void Transverse()
            => SetResult(NativeMagickImage.Transverse());

        public void UnsharpMask(double radius, double sigma)
            => UnsharpMask(radius, sigma, 1.0, 0.05);

        public void UnsharpMask(double radius, double sigma, Channels channels)
            => UnsharpMask(radius, sigma, 1.0, 0.05, channels);

        public void UnsharpMask(double radius, double sigma, double amount, double threshold)
            => UnsharpMask(radius, sigma, amount, threshold, ImageMagick.Channels.Undefined);

        public void UnsharpMask(double radius, double sigma, double amount, double threshold, Channels channels)
            => SetResult(NativeMagickImage.UnsharpMask(radius, sigma, amount, threshold, channels));

        public void Vignette()
            => Vignette(0.0, 1.0, 0, 0);

        public void Vignette(double radius, double sigma, int x, int y)
            => SetResult(NativeMagickImage.Vignette(radius, sigma, x, y));

        public void Wave()
            => Wave(NativeMagickImage.Interpolate_Get(), 25.0, 150.0);

        public void Wave(PixelInterpolateMethod method, double amplitude, double length)
            => SetResult(NativeMagickImage.Wave(method, amplitude, length));

        public void WaveletDenoise(QuantumType threshold)
            => WaveletDenoise(threshold, 0.0);

        public void WaveletDenoise(QuantumType threshold, double softness)
            => SetResult(NativeMagickImage.WaveletDenoise(threshold, softness));

        public void WaveletDenoise(Percentage thresholdPercentage)
            => WaveletDenoise(PercentageHelper.ToQuantumType(nameof(thresholdPercentage), thresholdPercentage));

        public void WaveletDenoise(Percentage thresholdPercentage, double softness)
            => WaveletDenoise(PercentageHelper.ToQuantumType(nameof(thresholdPercentage), thresholdPercentage), softness);

        protected virtual void SetResult(IntPtr result)
        {
            if (_result != IntPtr.Zero)
                throw new InvalidOperationException("Only a single operation can be executed.");

            _result = result;
        }

        private double Deskew(Percentage threshold, bool autoCrop)
        {
            using var temporaryDefines = new TemporaryDefines(NativeMagickImage);
            temporaryDefines.SetArtifact("deskew:auto-crop", autoCrop);

            SetResult(NativeMagickImage.Deskew(PercentageHelper.ToQuantum(nameof(threshold), threshold)));

            var artifact = NativeMagickImage.GetArtifact("deskew:angle");
            if (!double.TryParse(artifact, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return 0.0;

            return result;
        }
    }
}
