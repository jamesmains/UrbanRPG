using System;
using UnityEngine;

namespace i302.Utils.Events
{
	public class i302Event
	{
		public delegate void TheDelegate(); 
		event TheDelegate theEvent;

		public static i302Event operator +(i302Event lhs, TheDelegate rhs)
		{
			if (rhs != null)
			{
				lhs.theEvent += rhs;
			}

			return lhs;
		}

		public static i302Event operator -(i302Event lhs, TheDelegate rhs)
		{
			if (rhs != null)
			{
				lhs.theEvent -= rhs;
			}

			return lhs;
		}

		public void Raise()
		{

			if (theEvent != null)
			{
				foreach(TheDelegate handler in theEvent.GetInvocationList())
				{
					try
					{
						handler();
					}
					catch (Exception e)
					{
						Debug.LogError(e.Message + " \n " + e.StackTrace);
					}
				}
			} 
		}
	}

	public class i302Event<T>
	{
		public delegate void TheDelegate(T arg); 
		event TheDelegate theEvent;

		public static i302Event<T> operator +(i302Event<T> lhs, TheDelegate rhs)
		{
			if (rhs != null)
			{
				lhs.theEvent += rhs;
			}

			return lhs;
		}

		public static i302Event<T> operator -(i302Event<T> lhs, TheDelegate rhs)
		{
			if (rhs != null)
			{
				lhs.theEvent -= rhs;
			}

			return lhs;
		}

		public void Raise(T arg)
		{
			if (theEvent != null)
			{
				foreach(TheDelegate handler in theEvent.GetInvocationList())
				{
					try
					{
						handler(arg);
					}
					catch (Exception e)
					{
						Debug.LogError(e.Message + " \n " + e.StackTrace);
					}
				}
			} 
		}
	}

	public class i302Event<T,U>
	{
		public delegate void TheDelegate(T arg1, U arg2); 
		event TheDelegate theEvent;

		public static i302Event<T,U> operator +(i302Event<T,U> lhs, TheDelegate rhs)
		{
			if (rhs != null)
			{
				lhs.theEvent += rhs;
			}

			return lhs;
		}

		public static i302Event<T,U> operator -(i302Event<T,U> lhs, TheDelegate rhs)
		{
			if (rhs != null)
			{
				lhs.theEvent -= rhs;
			}

			return lhs;
		}

		public void Raise(T arg1, U arg2)
		{
			if (theEvent != null)
			{
				foreach(TheDelegate handler in theEvent.GetInvocationList())
				{
					try
					{
						handler(arg1, arg2);
					}
					catch (Exception e)
					{
						// One of the handlers had a problem
						Debug.LogError(e.Message + " \n " + e.StackTrace);
					}
				}
			} 
		}
	}

    public class i302Event<T, U, V>
    {
        public delegate void TheDelegate(T arg1, U arg2, V arg3);
        event TheDelegate theEvent;

        public static i302Event<T, U, V> operator +(i302Event<T, U, V> lhs, TheDelegate rhs)
        {
            if (rhs != null)
            {
                lhs.theEvent += rhs;
            }

            return lhs;
        }

        public static i302Event<T, U, V> operator -(i302Event<T, U, V> lhs, TheDelegate rhs)
        {
            if (rhs != null)
            {
                lhs.theEvent -= rhs;
            }

            return lhs;
        }

        public void Raise(T arg1, U arg2, V arg3)
        {
            if (theEvent != null)
            {
                foreach (TheDelegate handler in theEvent.GetInvocationList())
                {
                    try
                    {
                        handler(arg1, arg2, arg3);
                    }
                    catch (Exception e)
                    {
                        // One of the handlers had a problem
                        Debug.LogError(e.Message + " \n " + e.StackTrace);
                    }
                }
            }
        }
    }
}