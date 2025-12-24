using MrV.Geometry;
using System;

namespace MrV.CommandLine {
	public partial class DrawBuffer {
		public static ConsoleColorPair[,] AntiAliasColorMap;
		private Vec2 _originOffsetULCorner;
		public Vec2 Offset { get => _originOffsetULCorner; set => _originOffsetULCorner = value; }
		public Vec2 Scale {
			get => ShapeScale;
			set {
				Vec2 center = GetCameraCenter();
				ShapeScale = value;
				SetCameraCenter(center);
			}
		}
		public Vec2 GetCameraCenter() {
			Vec2 cameraCenterPercentage = (0.5f, 0.5f);
			Vec2 cameraCenterOffset = Size.Scaled(cameraCenterPercentage);
			Vec2 scaledCameraOffset = cameraCenterOffset.Scaled(Scale);
			Vec2 position = Offset + scaledCameraOffset;
			return position;
		}
		public void SetCameraCenter(Vec2 position) {
			Vec2 cameraCenterPercentage = (0.5f, 0.5f);
			Vec2 cameraCenterOffset = Size.Scaled(cameraCenterPercentage);
			Vec2 scaledCameraOffset = cameraCenterOffset.Scaled(Scale);
			Vec2 screenAnchor = position - scaledCameraOffset;
			Offset = screenAnchor;
		}
		static DrawBuffer() {
			int countColors = 16;
			int maxSuperSample = 4;
			AntiAliasColorMap = new ConsoleColorPair[countColors, maxSuperSample];
			for (int i = 0; i < countColors; ++i) {
				for (int s = 0; s < maxSuperSample; ++s) {
					AntiAliasColorMap[i, s] = new ConsoleColorPair(ConsoleColor.Gray, (ConsoleColor)i);
				}
			}
			// for light colors, the least-populated half should use the darker color
			for (int i = (int)ConsoleColor.Blue; i <= (int)ConsoleColor.White; ++i) {
				for (int s = 0; s < maxSuperSample/2; ++s) {
					AntiAliasColorMap[i, s] = new ConsoleColorPair(ConsoleColor.Gray, (ConsoleColor)(i-8));
				}
			}
			// gray is a special case because DarkGray is 1 value after Gray
			int grayIndex = (int)ConsoleColor.Gray;
			AntiAliasColorMap[grayIndex, 0] = new ConsoleColorPair(ConsoleColor.Gray, ConsoleColor.DarkGray);
			AntiAliasColorMap[grayIndex, 1] = new ConsoleColorPair(ConsoleColor.Gray, ConsoleColor.DarkGray);
		}

		public Vec2 ShapeScale = new Vec2(0.5f, 1);
		public delegate bool IsInsideShapeDelegate(Vec2 position);
		public void DrawShape(IsInsideShapeDelegate isInsideShape, Vec2 start, Vec2 end, ConsoleGlyph glyphToPrint) {
			Vec2 renderStart = start - _originOffsetULCorner;
			Vec2 renderEnd = end - _originOffsetULCorner;
			renderStart.InverseScale(ShapeScale);
			renderEnd.InverseScale(ShapeScale);
			int TotalSamplesPerGlyph = AntiAliasColorMap.GetLength(1);
			int SamplesPerDimension = (int)Math.Sqrt(TotalSamplesPerGlyph);
			float SuperSampleIncrement = 1f / SamplesPerDimension;
			for (int y = (int)renderStart.Y; y < renderEnd.Y; ++y) {
				for (int x = (int)renderStart.X; x < renderEnd.X; ++x) {
					if (!IsValidLocation(y, x)) { continue; }
					int countSamples = 0;
					if (isInsideShape != null) {
						for (float sampleY = 0; sampleY < 1; sampleY += SuperSampleIncrement) {
							for (float sampleX = 0; sampleX < 1; sampleX += SuperSampleIncrement) {
								bool pointIsInside = isInsideShape(new Vec2((x + sampleX) * ShapeScale.X, (y + sampleY) * ShapeScale.Y)
									+ _originOffsetULCorner);
								if (pointIsInside) {
									++countSamples;
								}
							}
						}
					} else {
						countSamples = TotalSamplesPerGlyph;
					}
					if (countSamples == 0) { continue; }
					ConsoleGlyph glyph = glyphToPrint;
					glyph.Back = AntiAliasColorMap[(int)glyphToPrint.Back, countSamples - 1].Back;
					WriteAt(glyph, y, x);
				}
			}
		}
		public void DrawRectangle(Vec2 position, Vec2 size, ConsoleGlyph letterToPrint) {
			DrawShape(null, position, position+size, letterToPrint);
		}
		public void DrawRectangle(int x, int y, int width, int height, ConsoleGlyph letterToPrint) {
			DrawShape(null, new Vec2(x, y), new Vec2(x + width, y + height), letterToPrint);
		}
		public void DrawRectangle(AABB aabb, ConsoleGlyph letterToPrint) {
			DrawShape(null, aabb.Min, aabb.Max, letterToPrint);
		}
		public void DrawCircle(Circle c, ConsoleGlyph letterToPrint) {
			// TODO why not use DrawShape, like DrawPolygon does?
			DrawCircle(c.center, c.radius, letterToPrint);
		}
		public void DrawCircle(Vec2 pos, float radius, ConsoleGlyph letterToPrint) {
			Vec2 extent = new Vec2(radius, radius);
			Vec2 start = pos - extent;
			Vec2 end = pos + extent;
			float r2 = radius * radius;
			DrawShape(IsInCircle, start, end, letterToPrint);
			bool IsInCircle(Vec2 point) {
				float dx = point.X - pos.X;
				float dy = point.Y - pos.Y;
				return dx * dx + dy * dy < r2;
			}
		}
		public void DrawPolygon(Vec2[] poly, ConsoleGlyph letterToPrint) {
			PolygonShape.TryGetAABB(poly, out Vec2 start, out Vec2 end);
			DrawShape(IsInPolygon, start, end, letterToPrint);
			bool IsInPolygon(Vec2 point) => PolygonShape.IsInPolygon(poly, point);
		}
		public void DrawLine(Vec2 start, Vec2 end, float thickness, ConsoleGlyph letterToPrint) {
			Vec2 delta = end - start;
			Vec2 direction = delta.Normalized();
			Vec2 perp = direction.Perpendicular() * thickness/2;
			Vec2[] line = new Vec2[] { start - perp, start + perp, end + perp, end - perp };
			DrawPolygon(line, letterToPrint); 
		}
	}
}
