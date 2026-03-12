using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007CF RID: 1999
	public class FileSystemElement
	{
		// Token: 0x17001614 RID: 5652
		// (get) Token: 0x060046B7 RID: 18103 RVA: 0x0036B4E7 File Offset: 0x003696E7
		// (set) Token: 0x060046B8 RID: 18104 RVA: 0x0036B4EF File Offset: 0x003696EF
		public FileSystemElementType ElementType
		{
			[CompilerGenerated]
			get
			{
				return this.<ElementType>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ElementType>k__BackingField = value;
			}
		}

		// Token: 0x17001615 RID: 5653
		// (get) Token: 0x060046B9 RID: 18105 RVA: 0x0036B4F8 File Offset: 0x003696F8
		// (set) Token: 0x060046BA RID: 18106 RVA: 0x0036B500 File Offset: 0x00369700
		public string Path
		{
			[CompilerGenerated]
			get
			{
				return this.<Path>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Path>k__BackingField = value;
			}
		}

		// Token: 0x17001616 RID: 5654
		// (get) Token: 0x060046BB RID: 18107 RVA: 0x0036B509 File Offset: 0x00369709
		// (set) Token: 0x060046BC RID: 18108 RVA: 0x0036B511 File Offset: 0x00369711
		public string Name
		{
			[CompilerGenerated]
			get
			{
				return this.<Name>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Name>k__BackingField = value;
			}
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x0036B51C File Offset: 0x0036971C
		public FileSystemElement(string path, FileSystemElementType type, bool isRoot = false)
		{
			if (type == FileSystemElementType.File)
			{
				this.Name = global::System.IO.Path.GetFileName(path);
			}
			else
			{
				this.Name = (isRoot ? "[ .. ]" : ("[ " + global::System.IO.Path.GetFileName(path) + " ]"));
			}
			this.Path = path;
			this.ElementType = type;
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x0036B574 File Offset: 0x00369774
		public void Delete()
		{
			try
			{
				if (this.ElementType == FileSystemElementType.File)
				{
					File.Delete(this.Path);
				}
				else
				{
					Directory.Delete(this.Path, true);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x0036B5B8 File Offset: 0x003697B8
		public void Rename(string new_name)
		{
			string text = global::System.IO.Path.Combine(global::System.IO.Path.GetDirectoryName(this.Path), new_name);
			File.Move(this.Path, text);
			this.Path = text;
			this.Name = new_name;
		}

		// Token: 0x060046C0 RID: 18112 RVA: 0x0036B5F1 File Offset: 0x003697F1
		public void Move(string new_full_path)
		{
			File.Move(this.Path, new_full_path);
			this.Path = new_full_path;
		}

		// Token: 0x0400290E RID: 10510
		[CompilerGenerated]
		private FileSystemElementType <ElementType>k__BackingField;

		// Token: 0x0400290F RID: 10511
		[CompilerGenerated]
		private string <Path>k__BackingField;

		// Token: 0x04002910 RID: 10512
		[CompilerGenerated]
		private string <Name>k__BackingField;
	}
}
