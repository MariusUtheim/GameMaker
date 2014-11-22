﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Randomness
{
	public static class Combinatorics
	{
		#region Tables and helper constants
		private const double LogTauOver2 = 0.918938533204673;
        private static readonly ulong[] factorialTable_0_to_20 = new ulong[] {
			1,
			1,
			2,
			6,
			24,
			120,
			720,
			5040,
			40320,
			362880,
			3628800,
			39916800,
			479001600,
			6227020800,
			87178291200,
			1307674368000,
			20922789888000,
			355687428096000,
			6402373705728000,
			121645100408832000,
			2432902008176640000 };

		private static readonly double[] logFactorialTable_0_to_255 = new double[] {
			0,
			0,
			0.693147180559945,
			1.79175946922806,
			3.17805383034795,
			4.78749174278205,
			6.57925121201010,
			8.52516136106541,
			10.6046029027453,
			12.8018274800815,
			15.1044125730755,
			17.5023078458739,
			19.9872144956619,
			22.5521638531234,
			25.1912211827387,
			27.8992713838409,
			30.6718601060807,
			33.5050734501369,
			36.3954452080331,
			39.3398841871995,
			42.3356164607535,
			45.3801388984769,
			48.4711813518352,
			51.6066755677644,
			54.7847293981123,
			58.0036052229805,
			61.2617017610020,
			64.5575386270063,
			67.8897431371815,
			71.2570389671680,
			74.6582363488302,
			78.0922235533153,
			81.5579594561150,
			85.0544670175815,
			88.5808275421977,
			92.1361756036871,
			95.7196945421432,
			99.3306124547874,
			102.968198614514,
			106.631760260643,
			110.320639714757,
			114.034211781462,
			117.771881399745,
			121.533081515439,
			125.317271149357,
			129.123933639127,
			132.952575035616,
			136.802722637326,
			140.673923648234,
			144.565743946345,
			148.477766951773,
			152.409592584497,
			156.360836303079,
			160.331128216631,
			164.320112263195,
			168.327445448428,
			172.352797139163,
			176.395848406997,
			180.456291417544,
			184.533828861449,
			188.628173423672,
			192.739047287845,
			196.866181672890,
			201.009316399282,
			205.168199482641,
			209.342586752537,
			213.532241494563,
			217.736934113954,
			221.956441819130,
			226.190548323728,
			230.439043565777,
			234.701723442818,
			238.978389561834,
			243.268849002983,
			247.572914096187,
			251.890402209723,
			256.221135550010,
			260.564940971863,
			264.921649798553,
			269.291097651020,
			273.673124285694,
			278.067573440366,
			282.474292687630,
			286.893133295427,
			291.323950094270,
			295.766601350761,
			300.220948647014,
			304.686856765669,
			309.164193580147,
			313.652829949879,
			318.152639620209,
			322.663499126726,
			327.185287703775,
			331.717887196928,
			336.261181979198,
			340.815058870799,
			345.379407062267,
			349.954118040770,
			354.539085519441,
			359.134205369575,
			363.739375555563,
			368.354496072405,
			372.979468885689,
			377.614197873919,
			382.258588773060,
			386.912549123218,
			391.575988217330,
			396.248817051792,
			400.930948278916,
			405.622296161145,
			410.322776526937,
			415.032306728250,
			419.750805599545,
			424.478193418257,
			429.214391866652,
			433.959323995015,
			438.712914186121,
			443.475088120919,
			448.245772745385,
			453.024896238496,
			457.812387981278,
			462.608178526875,
			467.412199571608,
			472.224383926981,
			477.044665492586,
			481.872979229888,
			486.709261136839,
			491.553448223298,
			496.405478487218,
			501.265290891579,
			506.132825342035,
			511.008022665236,
			515.890824587822,
			520.781173716044,
			525.679013515995,
			530.584288294433,
			535.496943180170,
			540.416924105998,
			545.344177791155,
			550.278651724286,
			555.220294146895,
			560.169054037273,
			565.124881094874,
			570.087725725134,
			575.057539024710,
			580.034272767131,
			585.017879388839,
			590.008311975618,
			595.005524249382,
			600.009470555327,
			605.020105849424,
			610.037385686239,
			615.061266207085,
			620.091704128477,
			625.128656730891,
			630.172081847810,
			635.221937855060,
			640.278183660408,
			645.340778693435,
			650.409682895655,
			655.484856710889,
			660.566261075874,
			665.653857411106,
			670.747607611913,
			675.847474039737,
			680.953419513637,
			686.065407301994,
			691.183401114411,
			696.307365093814,
			701.437263808737,
			706.573062245787,
			711.714725802290,
			716.862220279103,
			722.015511873601,
			727.174567172816,
			732.339353146739,
			737.509837141777,
			742.685986874351,
			747.867770424643,
			753.055156230484,
			758.248113081374,
			763.446610112640,
			768.650616799717,
			773.860102952558,
			779.075038710167,
			784.295394535246,
			789.521141208959,
			794.752249825813,
			799.988691788643,
			805.230438803703,
			810.477462875864,
			815.729736303910,
			820.987231675938,
			826.249921864843,
			831.517780023906,
			836.790779582470,
			842.068894241700,
			847.352097970438,
			852.640365001133,
			857.933669825857,
			863.231987192405,
			868.535292100465,
			873.843559797866,
			879.156765776908,
			884.474885770752,
			889.797895749890,
			895.125771918680,
			900.458490711945,
			905.796028791646,
			911.138363043611,
			916.485470574329,
			921.837328707805,
			927.193914982477,
			932.555207148186,
			937.921183163208,
			943.291821191336,
			948.667099599020,
			954.046996952560,
			959.431492015349,
			964.820563745166,
			970.214191291518,
			975.612353993036,
			981.015031374908,
			986.422203146368,
			991.833849198223,
			997.249949600428,
			1002.67048459970,
			1008.09543461718,
			1013.52478024614,
			1018.95850224969,
			1024.39658155861,
			1029.83899926914,
			1035.28573664080,
			1040.73677509437,
			1046.19209620972,
			1051.65168172387,
			1057.11551352889,
			1062.58357367003,
			1068.05584434370,
			1073.53230789563,
			1079.01294681897,
			1084.49774375247,
			1089.98668147862,
			1095.47974292196,
			1100.97691114726,
			1106.47816935780,
			1111.98350089373,
			1117.49288923036,
			1123.00631797653,
			1128.52377087299,
			1134.04523179085,
			1139.57068472998,
			1145.10011381750,
			1150.63350330622,
			1156.17083757324,
			1161.71210111840
		};
		#endregion

		public static int Combinations(int n, int k)
		{
			double c;

			if (n < 0)
				c = GMath.Pow(-1, k) * GMath.Exp(LogGamma(k - n) - (LogGamma(k + 1) + LogGamma(-n)));
			else
				c = GMath.Exp(LogGamma(n + 1) - (LogGamma(k + 1) + LogGamma(n - k + 1)));

			return checked((int)GMath.Round(c));
		}

		public static int Permutations(int n, int k)
		{
			double c = GMath.Exp(LogGamma(n + 1) - LogGamma(n - k + 1));
			return checked((int)GMath.Round(c));
		}

		private static void swap<T>(ref T[] src, int i, int j)
		{
			T tmp = src[i];
			src[i] = src[j];
			src[j] = tmp;
		}

		private static IEnumerable<IEnumerable<T>> _Permute<T>(T[] array, int startIndex)
		{
			if (startIndex == array.Length -1)
				yield return array;
			else
			{
				// Yield recursively
				foreach (var permutation in _Permute(array, startIndex + 1)) 
					yield return permutation;

				for (int i = startIndex; i < array.Length - 1; i++)
				{
					swap(ref array, startIndex, i + 1);
					foreach (var permutation in _Permute(array, startIndex + 1))
						yield return permutation;
				}

				// Restore subarray to original state
				var tmp = array[startIndex];
				for (int i = startIndex; i < array.Length - 1; i++)
					array[i] = array[i + 1];
				array[array.Length - 1] = tmp;

			}
		}

		public static IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<T> elements)
		{
			var originalArray = elements.ToArray();

			foreach (var permutation in _Permute(originalArray, 0))
				yield return permutation;
		}

		public static double LogGamma(int x)
		{
			if (x <= 0)
				return double.PositiveInfinity;
			if (x < 256)
				return logFactorialTable_0_to_255[x - 1];
			else
				return (x - 0.5) * GMath.Log(x) - x + LogTauOver2 + 1 / (12.0 * x);
		}

		public static ulong Factorial(int n)
		{
			if (n < 0)
				return 0;
			if (n > 20)
				throw new OverflowException("n! is not representable as a System.UInt64 for n > 20.");
			return factorialTable_0_to_20[n];
		}
	}
}
