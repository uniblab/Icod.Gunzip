﻿// Gunzip.exe decompresses files using the Deflate compression algorithm.
// Copyright( C ) 2025 Timothy J. Bruce

/*
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using System.Linq;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace Icod.Inflate {

	public static class Program {

		[System.STAThread]
		public static System.Int32 Main( System.String[] args ) {
			var processor = new Icod.Argh.Processor(
				new Icod.Argh.Definition[] {
					new Icod.Argh.Definition( "help", new System.String[] { "-h", "--help", "/help" } ),
					new Icod.Argh.Definition( "copyright", new System.String[] { "-c", "--copyright", "/copyright" } ),
				},
				System.StringComparer.OrdinalIgnoreCase
			);
			processor.Parse( args );

			var argLen = args.Length;
			if ( 
				( processor.Contains( "help" ) ) 
				|| ( 0 == argLen )
				|| ( 2 < argLen )
			) {
				PrintUsage();
				return 1;
			} else if ( processor.Contains( "copyright" ) ) {
				PrintCopyright();
				return 1;
			}

			System.String inputPathName = args[ 0 ];
			System.String outputPathName;
			if (2 == argLen ) {
				outputPathName = args[ 1 ];
				if ( System.IO.Directory.Exists( outputPathName ) ) {
					var name = System.IO.Path.GetFileName( inputPathName );
					if ( name.EndsWith( ".gzip", System.StringComparison.OrdinalIgnoreCase ) ) {
						name = System.IO.Path.GetFileNameWithoutExtension( name );
					}
					outputPathName = System.IO.Path.Combine( outputPathName, name );
				}
			} else {
				var name = System.IO.Path.GetFileName( inputPathName );
				if ( name.EndsWith( ".gzip", System.StringComparison.OrdinalIgnoreCase ) ) {
					name = System.IO.Path.GetFileNameWithoutExtension( name );
				}
				outputPathName = System.IO.Path.Combine( System.Environment.CurrentDirectory, name );
			}
			var output = 0;
			try {
				Decompress( inputPathName, outputPathName );
			} catch ( System.Exception e ) {
				System.Console.Error.WriteLine( e.Message );
				output = 1;
			}
			return output;
		}

		private static void Decompress( System.String inputFilePathName, System.String outputFilePathName ) {
			using ( var reader = System.IO.File.OpenRead( inputFilePathName ) ) {
				using ( var output = System.IO.File.OpenWrite( outputFilePathName ) ) {
					using ( var worker = new System.IO.Compression.GZipStream( reader, System.IO.Compression.CompressionMode.Decompress, true ) ) {
						worker.CopyTo( output );
						worker.Flush();
						worker.Close();
					}
					output.Flush();
					output.SetLength( output.Position );
					output.Close();
				}
				reader.Close();
			}
		}

		private static void PrintUsage() {
			System.Console.Error.WriteLine( "No, no, no! Use it like this, Einstein:" );
			System.Console.Error.WriteLine( "Gunzip.exe --help" );
			System.Console.Error.WriteLine( "Gunzip.exe --copyright" );
			System.Console.Error.WriteLine( "Gunzip.exe inputFilePathName [outputFilePathNam]" );
			System.Console.Error.WriteLine( "inputFilePathName and outputFilePathName may be relative or absolute paths." );
			System.Console.Error.WriteLine( "If outputFilePathName is omitted then it will be generated in the current directory and named the same as inputFilePathName but with '.gzip' removed from the end if present." );
			System.Console.Error.WriteLine( "If the outputFilePathName is an existing directory, then it will be generated in that directory and named the same as inputFilePathName but with '.gzip' removed from the end if present." );
		}
		private static void PrintCopyright() {
			var copy = new System.String[] {
				"Gunzip.exe decompresses files using the Deflate compression algorithm.",
				"Copyright( C ) 2025 Timothy J. Bruce",
				"",
				"",
				"This program is free software: you can redistribute it and / or modify",
				"it under the terms of the GNU General Public License as published by",
				"the Free Software Foundation, either version 3 of the License, or",
				"( at your option ) any later version.",
				"",
				"This program is distributed in the hope that it will be useful,",
				"but WITHOUT ANY WARRANTY; without even the implied warranty of",
				"MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the",
				"GNU General Public License for more details.",
				"",
				"You should have received a copy of the GNU General Public License",
				"along with this program.If not, see <https://www.gnu.org/licenses/>."
			};
			foreach ( var line in copy) {
				System.Console.WriteLine( line );
			}
		}

	}

}