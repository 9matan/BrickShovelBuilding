using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	namespace Events
	{
		public delegate void OnTimeAction();
	}

	public interface IBSBTimeManager :
		IBSBTimeManagerEvents
	{
		int year { get; }
		int month { get; }
		int day { get; }
		int totalDays { get; }
		
		bool pause { get; set; }
	}

	public interface IBSBTimeManagerEvents
	{
		event Events.OnTimeAction onDayEnded;
		event Events.OnTimeAction onMonthEnded;
		event Events.OnTimeAction onYearEnded;
	}

	public class BSBTimeManager : MonoBehaviour,
		IBSBTimeManager,
		IVOSInitializable 
	{
		
		public int year
		{
			get { return _years; }
		}
		public int month
		{
			get { return _yearMonth; }
		}
		public int day
		{
			get { return _monthDay; }
		}
		public int totalDays
		{
			get { return _totalDays; }
		}

		public bool pause
		{
			get { return _pause; }
			set { _pause = value; }
		}

		[SerializeField]
		protected float _dayDuration = 1.0f;
		[SerializeField]
		protected int _daysPerMonth = 30;
		[SerializeField]
		protected int _monthsPerYear = 12;
		[SerializeField]
		protected bool _pause;

		[SerializeField]
		protected int _totalDays = 0;
		[SerializeField]
		protected int _monthDay = 0;
		[SerializeField]
		protected int _yearMonth = 0;
		[SerializeField]
		protected int _years = 0;
		
		public void Initialize()
		{
			_pause = false;
		}



		protected float _currentTime = 0.0f;

		protected void Update()
		{
			if (_pause) return;

			_currentTime += Time.deltaTime;

			if(_currentTime >= _dayDuration)
			{
				_OnDayTick();
				_currentTime = 0.0f;
			}
		}

		protected void _OnDayTick()
		{
			++_totalDays;
			++_monthDay;

			if (_monthDay == _daysPerMonth)
			{
				_monthDay = 0;
				_OnMonthTick();
			}

			_OnDayEnded();
		}

		protected void _OnMonthTick()
		{
			++_yearMonth;

			if (_yearMonth == _monthsPerYear)
			{
				_yearMonth = 0;
				_OnYearTick();
			}

			_OnMonthEnded();
		}

		protected void _OnYearTick()
		{
			++_years;
			_OnYearEnded();
		}

		//
		// < Events >
		//

		public event Events.OnTimeAction onDayEnded = delegate { };
		public event Events.OnTimeAction onMonthEnded = delegate { };
		public event Events.OnTimeAction onYearEnded = delegate { };

		protected void _OnDayEnded()
		{
			onDayEnded();
		}

		protected void _OnMonthEnded()
		{
			Log("Month ended");
			onMonthEnded();
		}

		protected void _OnYearEnded()
		{			
			onYearEnded();
		}

		//
		// </ Events >
		//

		//
		// < Log >
		//

		public bool debug = false;
				
		public void Log(object msg)
		{
			if(debug)
				Debug.Log(msg);
		}		

		//
		// </ Log >
		//
		
	}

}
