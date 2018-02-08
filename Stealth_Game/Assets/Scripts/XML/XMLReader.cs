using System;
using UnityEngine;
using System.Collections.Generic;
using System.Xml;

namespace RhythmStealth
{

public static class XMLReader{
	public static Dictionary<int,RhythmEvent> ReadSoldierTypeFile(string path)
	{
		Dictionary<int,RhythmEvent> rhythmEventList = new Dictionary<int, RhythmEvent>();
		XmlDocument xDoc = new XmlDocument();
			// Android hack fix that doesn't require filepath
//			if (Application.platform == RuntimePlatform.Android) {
//				TextAsset soldierXML = Resources.Load<TextAsset> ("XML/Soldiers");
//				xDoc.LoadXml (soldierXML.text);
//			} else {
//				xDoc.Load (path);
//			}
			TextAsset soldierXML = Resources.Load<TextAsset> ("XML/RhythmEvents1");
			Debug.Log (soldierXML.text);
			xDoc.LoadXml (soldierXML.text);
		XmlNamespaceManager xnm = new XmlNamespaceManager(xDoc.NameTable);
		xnm.AddNamespace("WB", "urn:schemas-microsoft-com:office:spreadsheet");
		XmlElement root = xDoc.DocumentElement;
		XmlNodeList rows = root.SelectNodes("/WB:Workbook/WB:Worksheet/WB:Table/WB:Row", xnm);

		for (int i = 3; i < rows.Count; i++)
		{
			XmlElement rowNode = rows[i] as XmlElement;
			if (rowNode != null)
			{
				RhythmEvent newRhythemEvent = new RhythmEvent();
				//评论ID
				//Debug.LogWarning(GetInnerData(rowNode.ChildNodes[0]));
				newRhythemEvent.EventTypeID = int.Parse(GetInnerData(rowNode.ChildNodes[0]));

				newRhythemEvent.EventName = GetInnerData(rowNode.ChildNodes[1]);
				string eventTimeString = GetInnerData(rowNode.ChildNodes[2]);
				string[] eventChar= eventTimeString.Split(';');
				newRhythemEvent.EventTime=Array.ConvertAll(eventChar, float.Parse);

				newRhythemEvent.EffectDuration = float.Parse(GetInnerData(rowNode.ChildNodes[3]));


				/*string GapTimeString = GetInnerData(rowNode.ChildNodes[10]);
                string[] GapTimeStringArray = GapTimeString.Split('+');

                for (int f=0;f < GapTimeStringArray.Length;f++) 
                  {
                    newComment.CharacterGapTime[f] = float.Parse(GapTimeStringArray[f]);
                }*/


				rhythmEventList.Add(newRhythemEvent.EventTypeID,newRhythemEvent);
			}
		}
		return rhythmEventList;
	}
/*
	public static Dictionary<int,Fraction> ReadFractionsFile(string path)
	{
		Dictionary<int,Fraction> fractionsList = new Dictionary<int, Fraction>();
		XmlDocument xDoc = new XmlDocument();
//			if (Application.platform == RuntimePlatform.Android) {
//				TextAsset fractionXML = Resources.Load<TextAsset> ("XML/Fractions");
//				xDoc.LoadXml (fractionXML.text);
//			} else {
//				xDoc.Load (path);
//			}

			TextAsset fractionXML = Resources.Load<TextAsset> ("XML/Fractions");
			xDoc.LoadXml (fractionXML.text);
		XmlNamespaceManager xnm = new XmlNamespaceManager(xDoc.NameTable);
		xnm.AddNamespace("WB", "urn:schemas-microsoft-com:office:spreadsheet");
		XmlElement root = xDoc.DocumentElement;
		XmlNodeList rows = root.SelectNodes("/WB:Workbook/WB:Worksheet/WB:Table/WB:Row", xnm);

		for (int i = 3; i < rows.Count; i++)
		{
			XmlElement rowNode = rows[i] as XmlElement;
			if (rowNode != null)
			{
				Fraction newFraction = new Fraction();
				//评论ID
				newFraction.FractionID = int.Parse(GetInnerData(rowNode.ChildNodes[0]));

				newFraction.FractionsMaxHP = int.Parse(GetInnerData(rowNode.ChildNodes[1]));

				newFraction.FractionsInitHP= int.Parse(GetInnerData(rowNode.ChildNodes[2]));
				int WinConditionHPInt=int.Parse( GetInnerData(rowNode.ChildNodes[3]));
				newFraction.WinConditionHP = WinConditionHPInt == 1 ? true : false;
				fractionsList.Add(newFraction.FractionID,newFraction);
			}
		}
		return fractionsList;
	}
*/


	private static string GetInnerData(XmlNode node) {
		if (node.ChildNodes[0] != null)
		{
			return node.ChildNodes[0].InnerText;
		}
		else {
			return string.Empty;
		}
	}


}
	
}

