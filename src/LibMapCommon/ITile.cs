﻿using LibMapCommon.Geometry;

namespace LibMapCommon;

public interface ITile<TTile, TCoordinate> : ITile<TCoordinate> where TCoordinate : IGeoCoordinate<TCoordinate>
{
	static abstract TTile GetTile(TCoordinate coordinate, int level);
	static abstract TTile Create(int row, int col, int level);
}

public interface ITile<TCoordinate> : ITile where TCoordinate : IGeoCoordinate<TCoordinate>
{
	/// <summary> The lower-left (southwest) coordinate of this <see cref="ITile{TCoordinate}"/> </summary>
	TCoordinate LowerLeft { get; }
	/// <summary> The lower-right (southeast) coordinate of this <see cref="ITile{TCoordinate}"/> </summary>
	TCoordinate LowerRight { get; }
	/// <summary> The upper-left (northwest) coordinate of this <see cref="ITile{TCoordinate}"/> </summary>
	TCoordinate UpperLeft { get; }
	/// <summary> The upper-right (northeast) coordinate of this <see cref="ITile{TCoordinate}"/> </summary>
	TCoordinate UpperRight { get; }
	/// <summary> coordinate of the center of this <see cref="ITile{TCoordinate}"/> </summary>
	TCoordinate Center { get; }
	GeoPolygon<TCoordinate> GetGeoPolygon();
}

public interface ITile
{
	int Row { get; }
	/// <summary> The number of <see cref="ITile"/> columns from the left-most (west-most) edge of the map. </summary>
	int Column { get; }
	/// <summary> The <see cref="ITile"/>'s zoom level. </summary>
	int Level { get; }
	/// <summary> coordinate of the center of this <see cref="ITile{TCoordinate}"/> </summary>
	Wgs1984 Wgs84Center { get; }
}
