﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace NSUListView
{
	public enum Layout
	{
		Vertical,
		Horizontal
	}
	
	public enum Alignment
	{
		Left,
		Mid,
		Right,
		Top,
		Bottom
	}

	public class USimpleListView : IUListView
	{
		public Layout 				layout;
		public Alignment 			alignment;
		protected List<GameObject>	lstItems;

		public override void Init ()
		{
			base.Init ();
			switch (layout) 
			{
			case Layout.Horizontal:
				scrollRect.horizontal = true;
				scrollRect.vertical = false;
				break;
			case Layout.Vertical:
				scrollRect.horizontal = false;
				scrollRect.vertical = true;
				break;
			}
			lstItems = new List<GameObject> ();
		}

		public override int	GetMaxShowItemNum()
		{
			int max = 0;
			// calculate the max show nums
			switch (layout) 
			{
			case Layout.Horizontal:
				max = (int)(scrollRectSize.x / itemSize.x) + 2;
				break;
			case Layout.Vertical:
				max = (int)(scrollRectSize.y / itemSize.y) + 2;
				break;
			}
			return max;
		}

		public override int GetStartIndex()
		{
			Vector2 anchorPosition = content.anchoredPosition;
			anchorPosition.x *= -1;
			int index = 0;

			switch (layout)
			{
			case Layout.Vertical:
				index = (int)(anchorPosition.y / itemSize.y);
				break;
			case Layout.Horizontal:
				index = (int)(anchorPosition.x / itemSize.x);
				break;
			}

			return index;
		}

		public override Vector2 GetItemAnchorPos(int index)
		{
			Vector2 basePos = Vector2.zero;
			Vector2 offset = Vector2.zero;
			RectTransform contentRectTransform = content.transform as RectTransform;
			Vector2 contentRectSize = contentRectTransform.rect.size;

			if (layout == Layout.Horizontal) 
			{
				basePos.x = -contentRectSize.x / 2 + itemSize.x / 2;
				offset.x = index * (itemSize.x + pad.x);
				switch(alignment)
				{
				case Alignment.Top:
					offset.y = -(contentRectSize.y - itemSize.y)/2;
					break;
				case Alignment.Bottom:
					offset.y = (contentRectSize.y - itemSize.y)/2;
					break;
				}
			} 
			else 
			{
				basePos.y = contentRectSize.y / 2 - itemSize.y / 2;
				offset.y = -index * (itemSize.y + pad.y);
				switch(alignment)
				{
				case Alignment.Left:
					offset.x = -(contentRectSize.x - itemSize.x)/2;
					break;
				case Alignment.Right:
					offset.x = (contentRectSize.x - itemSize.x)/2;
					break;
				}
			}

			return basePos + offset;
		}

		public override Vector2 GetContentSize()
		{
			Vector2 size = scrollRectSize;
			int count = lstData.Count;
			switch (layout) 
			{
			case Layout.Horizontal:
				size.x = itemSize.x * count + pad.x *( count > 0 ? count -1 : count );
				break;
			case Layout.Vertical:
				size.y = itemSize.y * count + pad.y * ( count > 0 ? count - 1 : count );
				break;
			}
			return size;
		}

		public override GameObject GetItemGameObject(int index)
		{
			if(index < lstItems.Count)
			{
				return lstItems [index];
			}
			else 
			{
				GameObject go = GameObject.Instantiate(item) as GameObject;
				lstItems.Add (go);
				return go;
			}
		}
	}
}