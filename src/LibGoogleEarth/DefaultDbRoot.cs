﻿using Keyhole;
using Keyhole.Dbroot;

namespace LibGoogleEarth;

internal class DefaultDbRoot : DbRoot
{
	public override Database Database => Database.Default;
	public static string DatabaseUrl => "https://khmdb.google.com/dbRoot.v5?&hl=en&gl=us&output=proto";

	internal DefaultDbRoot(DirectoryInfo cacheDir, EncryptedDbRootProto dbRootEnc)
		: base(cacheDir, dbRootEnc) { }

	protected override async Task<IQuadtreePacket> GetPacketAsync(Tile tile, int epoch)
	{
		const string QP2 = "https://kh.google.com/flatfile?q2-{0}-q.{1}";

		var url = string.Format(QP2, tile.Path, epoch);
		byte[] compressedPacket = await DownloadBytesAsync(url);
		byte[] decompressedPacket = DecompressBuffer(compressedPacket);

		return KhQuadTreePacket16.ParseFrom(decompressedPacket, tile.IsRoot);
	}
}
