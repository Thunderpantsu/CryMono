﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public static class TimeOfDay
	{
		/// <summary>
		/// Controls whether Time of Day updates take effect immediately.
		/// </summary>
		public static bool ForceUpdates { get; set; }

		/// <summary>
		/// The hour value for the Time of Day system.
		/// The value is wrapped, so setting the value to 24 will reset the hour to zero.
		/// </summary>
		public static int Hour
		{
			get
			{
				return (int)Engine._GetTimeOfDay();
			}
			set
			{
				while(value >= 24)
				{
					value -= 24;
				}
				while(value < 0)
				{
					value += 24;
				}

				RawEngineTime = CreateEngineTime(value, Minute);
			}
		}

		/// <summary>
		/// The minute value for the Time of Day system.
		/// The value is wrapped, so setting the value to 60 will increment the hour and reset the minutes to zero.
		/// </summary>
		public static int Minute
		{
			get
			{
				return GetMinutes(Engine._GetTimeOfDay());
			}
			set
			{
				RawEngineTime = CreateEngineTime(Hour, value);
			}
		}

		/// <summary>
		/// Controls the speed at which the Time of Day passes.
		/// </summary>
		public static float Speed
		{
			get
			{
				return Engine._GetTimeOfDayAdvancedInfo().fAnimSpeed;
			}
			set
			{
				var info = Engine._GetTimeOfDayAdvancedInfo();
				info.fAnimSpeed = value;
				Engine._SetTimeOfDayAdvancedInfo(info);
			}
		}

		/// <summary>
		/// Gets the minute value from a CE-style time float
		/// </summary>
		/// <param name="ceTime"></param>
		/// <returns></returns>
		internal static int GetMinutes(float ceTime)
		{
			return (int)System.Math.Round((ceTime - (int)ceTime) * 60);
		}

		/// <summary>
		/// Gets the hour value from a CE-style time float
		/// </summary>
		/// <param name="ceTime"></param>
		/// <returns></returns>
		internal static int GetHours(float ceTime)
		{
			return (int)ceTime;
		}

		/// <summary>
		/// Creates a CE-style time from a given number of hours and minutes
		/// </summary>
		/// <param name="hours"></param>
		/// <param name="mins"></param>
		/// <returns></returns>
		internal static float CreateEngineTime(int hours, int mins)
		{
			return hours + ((float)mins / 60);
		}

		/// <summary>
		/// Convenient accessor for the raw time value
		/// </summary>
		internal static float RawEngineTime
		{
			get
			{
				return Engine._GetTimeOfDay();
			}
			set
			{
				Engine._SetTimeOfDay(value, ForceUpdates);
			}
		}

		internal struct AdvancedInfo
		{
			public float fStartTime;
			public float fEndTime;
			public float fAnimSpeed;
		}

		internal enum ParamId
		{
			PARAM_HDR_DYNAMIC_POWER_FACTOR,

			PARAM_TERRAIN_OCCL_MULTIPLIER,
			PARAM_SSAO_MULTIPLIER,
			PARAM_SSAO_CONTRAST_MULTIPLIER,
			PARAM_GI_MULTIPLIER,

			PARAM_SUN_COLOR,
			PARAM_SUN_COLOR_MULTIPLIER,
			PARAM_SUN_SPECULAR_MULTIPLIER,

			PARAM_SKY_COLOR,
			PARAM_SKY_COLOR_MULTIPLIER,

			PARAM_AMBIENT_GROUND_COLOR,
			PARAM_AMBIENT_GROUND_COLOR_MULTIPLIER,

			PARAM_AMBIENT_MIN_HEIGHT,
			PARAM_AMBIENT_MAX_HEIGHT,

			PARAM_FOG_COLOR,
			PARAM_FOG_COLOR_MULTIPLIER,
			PARAM_VOLFOG_HEIGHT,
			PARAM_VOLFOG_DENSITY,
			PARAM_FOG_COLOR2,
			PARAM_FOG_COLOR2_MULTIPLIER,
			PARAM_VOLFOG_HEIGHT2,
			PARAM_VOLFOG_DENSITY2,
			PARAM_VOLFOG_HEIGHT_OFFSET,

			PARAM_FOG_RADIAL_COLOR,
			PARAM_FOG_RADIAL_COLOR_MULTIPLIER,
			PARAM_VOLFOG_RADIAL_SIZE,
			PARAM_VOLFOG_RADIAL_LOBE,

			PARAM_VOLFOG_FINAL_DENSITY_CLAMP,

			PARAM_VOLFOG_GLOBAL_DENSITY,
			PARAM_VOLFOG_RAMP_START,
			PARAM_VOLFOG_RAMP_END,
			PARAM_VOLFOG_RAMP_INFLUENCE,

			PARAM_SKYLIGHT_SUN_INTENSITY,
			PARAM_SKYLIGHT_SUN_INTENSITY_MULTIPLIER,

			PARAM_SKYLIGHT_KM,
			PARAM_SKYLIGHT_KR,
			PARAM_SKYLIGHT_G,

			PARAM_SKYLIGHT_WAVELENGTH_R,
			PARAM_SKYLIGHT_WAVELENGTH_G,
			PARAM_SKYLIGHT_WAVELENGTH_B,

			PARAM_NIGHSKY_HORIZON_COLOR,
			PARAM_NIGHSKY_HORIZON_COLOR_MULTIPLIER,
			PARAM_NIGHSKY_ZENITH_COLOR,
			PARAM_NIGHSKY_ZENITH_COLOR_MULTIPLIER,
			PARAM_NIGHSKY_ZENITH_SHIFT,

			PARAM_NIGHSKY_START_INTENSITY,

			PARAM_NIGHSKY_MOON_COLOR,
			PARAM_NIGHSKY_MOON_COLOR_MULTIPLIER,
			PARAM_NIGHSKY_MOON_INNERCORONA_COLOR,
			PARAM_NIGHSKY_MOON_INNERCORONA_COLOR_MULTIPLIER,
			PARAM_NIGHSKY_MOON_INNERCORONA_SCALE,
			PARAM_NIGHSKY_MOON_OUTERCORONA_COLOR,
			PARAM_NIGHSKY_MOON_OUTERCORONA_COLOR_MULTIPLIER,
			PARAM_NIGHSKY_MOON_OUTERCORONA_SCALE,

			PARAM_CLOUDSHADING_SUNLIGHT_MULTIPLIER,
			PARAM_CLOUDSHADING_SKYLIGHT_MULTIPLIER,
			PARAM_CLOUDSHADING_SUNLIGHT_CUSTOM_COLOR,
			PARAM_CLOUDSHADING_SUNLIGHT_CUSTOM_COLOR_MULTIPLIER,
			PARAM_CLOUDSHADING_SUNLIGHT_CUSTOM_COLOR_INFLUENCE,
			PARAM_CLOUDSHADING_SKYLIGHT_CUSTOM_COLOR,
			PARAM_CLOUDSHADING_SKYLIGHT_CUSTOM_COLOR_MULTIPLIER,
			PARAM_CLOUDSHADING_SKYLIGHT_CUSTOM_COLOR_INFLUENCE,

			PARAM_SUN_SHAFTS_VISIBILITY,
			PARAM_SUN_RAYS_VISIBILITY,
			PARAM_SUN_RAYS_ATTENUATION,
			PARAM_SUN_RAYS_SUNCOLORINFLUENCE,
			PARAM_SUN_RAYS_CUSTOMCOLOR,

			PARAM_OCEANFOG_COLOR,
			PARAM_OCEANFOG_COLOR_MULTIPLIER,
			PARAM_OCEANFOG_DENSITY,

			PARAM_SKYBOX_MULTIPLIER,

			PARAM_HDR_FILMCURVE_SHOULDER_SCALE,
			PARAM_HDR_FILMCURVE_LINEAR_SCALE,
			PARAM_HDR_FILMCURVE_TOE_SCALE,
			PARAM_HDR_FILMCURVE_WHITEPOINT,

			PARAM_HDR_BLUE_SHIFT,
			PARAM_HDR_BLUE_SHIFT_THRESHOLD,
			PARAM_HDR_COLORGRADING_COLOR_SATURATION,
			PARAM_HDR_COLORGRADING_COLOR_CONTRAST,
			PARAM_HDR_COLORGRADING_COLOR_BALANCE,

			PARAM_COLORGRADING_COLOR_SATURATION,
			PARAM_COLORGRADING_COLOR_CONTRAST,
			PARAM_COLORGRADING_COLOR_BRIGHTNESS,

			PARAM_COLORGRADING_LEVELS_MININPUT,
			PARAM_COLORGRADING_LEVELS_GAMMA,
			PARAM_COLORGRADING_LEVELS_MAXINPUT,
			PARAM_COLORGRADING_LEVELS_MINOUTPUT,
			PARAM_COLORGRADING_LEVELS_MAXOUTPUT,

			PARAM_COLORGRADING_SELCOLOR_COLOR,
			PARAM_COLORGRADING_SELCOLOR_CYANS,
			PARAM_COLORGRADING_SELCOLOR_MAGENTAS,
			PARAM_COLORGRADING_SELCOLOR_YELLOWS,
			PARAM_COLORGRADING_SELCOLOR_BLACKS,

			PARAM_COLORGRADING_FILTERS_GRAIN,
			PARAM_COLORGRADING_FILTERS_SHARPENING,
			PARAM_COLORGRADING_FILTERS_PHOTOFILTER_COLOR,
			PARAM_COLORGRADING_FILTERS_PHOTOFILTER_DENSITY,

			PARAM_COLORGRADING_DOF_FOCUSRANGE,
			PARAM_COLORGRADING_DOF_BLURAMOUNT,

			PARAM_SHADOWSC0_BIAS,
			PARAM_SHADOWSC0_SLOPE_BIAS,
			PARAM_SHADOWSC1_BIAS,
			PARAM_SHADOWSC1_SLOPE_BIAS,
			PARAM_SHADOWSC2_BIAS,
			PARAM_SHADOWSC2_SLOPE_BIAS,
			PARAM_SHADOWSC3_BIAS,
			PARAM_SHADOWSC3_SLOPE_BIAS,

			PARAM_TOTAL
		}
	}
}
