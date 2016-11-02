using UnityEngine;
using System.Collections;

namespace VVOSS.Tools.FileTools
{

	public interface IVOSFileManager :
	IVOSXmlSerializer
	{
		string GetPersistentFilePath(string file);
	}

	public interface IVOSXmlSerializer
	{
		void XmlSerialize<TObject>(string path, TObject instance)
			where TObject : class;
		TObject XmlDeserialize<TObject>(string path)
			where TObject : class;
	}

	public class VOSFileManager :
		IVOSFileManager
	{

		public virtual string GetPersistentFilePath(string file)
		{
			return string.Format("{0}/{1}", Application.persistentDataPath, file);
		}

		//
		// < Xml >
		//

		public virtual void XmlSerialize<TObject>(string path, TObject instance)
			where TObject : class
		{
			var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TObject));
			var stream = new System.IO.FileStream(path, System.IO.FileMode.Create);
			serializer.Serialize(stream, instance);
			stream.Close();
		}

		public virtual TObject XmlDeserialize<TObject>(string path)
			where TObject : class
		{
			var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TObject));
			var stream = new System.IO.FileStream(path, System.IO.FileMode.Open);
			TObject instance = serializer.Deserialize(stream) as TObject;
			stream.Close();
			return instance;
		}

		//
		// </ Xml >
		//

		public virtual void WriteToFile(string fileName, string text)
		{
			var writer = new System.IO.StreamWriter(fileName);
			writer.Write(text);
			writer.Close();
		}

	}

}