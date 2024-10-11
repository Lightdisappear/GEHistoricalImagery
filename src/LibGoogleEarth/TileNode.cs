﻿using Keyhole;

namespace LibGoogleEarth;

/// <summary>
/// Relates a <see cref="Keyhole.QuadtreeNode"/> to a <see cref="LibGoogleEarth.Tile"/>
/// </summary>
public class TileNode
{
	private const int MIN_JPEG_DATE = 545;
	/// <summary> The Google Earth quadtree node </summary>
	public IQuadtreeNode QuadtreeNode { get; }
	/// <summary> The <see cref="LibGoogleEarth.Tile"/> associated with the <see cref="QuadtreeNode"/> </summary>
	public Tile Tile { get; }

	internal TileNode(Tile tile, IQuadtreeNode quadtreeNode)
	{
		QuadtreeNode = quadtreeNode;
		Tile = tile;
	}

	public bool HasTerrain()
	=> QuadtreeNode.Layer.Any(l => l.Type is QuadtreeLayer.Types.LayerType.Terrain);

	public TerrainTile? GetTerrain()
	{
		var terrainLayer = QuadtreeNode.Layer.SingleOrDefault(l => l.Type is QuadtreeLayer.Types.LayerType.Terrain);

		return terrainLayer == null ? null
			: new TerrainTile(Tile, terrainLayer);
	}


	/// <summary>
	/// Determines whether this quadtree node has imagery available from a specific date.
	/// </summary>
	/// <param name="dateOnly">A specific date</param> 
	/// <returns><see langword="true"/> if the quadtree node has imagery available from the date; otherwise, <see langword="false"/>.</returns>
	public bool HasDate(DateOnly dateOnly)
	=> GetAllDatedTiles().Any(dt => dt.Date == dateOnly);

	/// <summary>
	/// Returns an enumerable collection of all <see cref="DatedTile"/>s present in the <see cref="QuadtreeNode"/>
	/// </summary>
	public IEnumerable<DatedTile> GetAllDatedTiles()
	{
		if (QuadtreeNode is not QuadtreeNode node)
			yield break;

		var datesLayer
			= node
			?.Layer
			?.FirstOrDefault(l => l.Type is QuadtreeLayer.Types.LayerType.ImageryHistory)
			?.DatesLayer
			.DatedTile;

		if (datesLayer == null)
			yield break;

		foreach (var dt in datesLayer)
		{
			if (dt.Date <= MIN_JPEG_DATE)
				continue;
			else if (dt.Provider != 0)
				yield return new DatedTile(Tile, dt);
			//When Provider is zero, that tile's imagery is being used as the default and is in the Imagery layer.
			else if (node?.Layer?.FirstOrDefault(l => l.Type is QuadtreeLayer.Types.LayerType.Imagery) is QuadtreeLayer regImagery)
				yield return new DatedTile(Tile, dt.DateOnly, regImagery);
		}
	}
}
