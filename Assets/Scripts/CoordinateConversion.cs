using System;
using IUX;

/// <summary>
/// Coordinate conversions between GEO(LLA), ECEF and ENU
/// </summary>
public static class CoordinateConversion
{
    private const double a = 6378137.0;               //WGS-84 semi-major axis
    private const double e2 = 6.6943799901377997e-3;  //WGS-84 first eccentricity squared

    private const double deg2rad = Math.PI / 180.0;

    /// <summary>
    /// Coverts From GEO (Lat Lng Alt, degrees & meters) coordinates to ECEF (Earth-centered Earth-fixed) coordinates
    /// </summary>
    /// <param name="geo">Point in GEO coordinates</param>
    /// <returns>Point in ECEF coordinates</returns>
    public static Point3 GEO_to_ECEF(Point3 geo)
    {
        double n, lat, lon, alt;
        Point3 ecef = new Point3();

        lat = geo.X * deg2rad;
        lon = geo.Y * deg2rad;
        alt = geo.Z;

        n = a / Math.Sqrt(1 - e2 * Math.Sin(lat) * Math.Sin(lat));
        ecef.X = (n + alt) * Math.Cos(lat) * Math.Cos(lon);
        ecef.Y = (n + alt) * Math.Cos(lat) * Math.Sin(lon); 
        ecef.Z = (n * (1 - e2) + alt) * Math.Sin(lat);

        return ecef;
    }

    /// <summary>
	/// Converts from ECEF (Earth-centered Earth-fixed, meters) coordinates to ENU (East North Up, meters) coordinates, relative to the reference point
	/// </summary>
	/// <param name="ecef">Point in ECEF coordinates</param>
	/// <param name="refLat">Reference point latitude, degrees</param>
	/// <param name="refLon">Reference point longitude, degrees</param>
	/// <param name="ecefRef">Reference point ECEF coordinates</param>
	/// <returns>Point in ENU coordinates</returns>
    public static Point3 ECEF_TO_ENU(Point3 ecef, double refLat, double refLon, Point3 ecefRef)
    {
        Point3 diff = new Point3 { X = ecef.X - ecefRef.X, Y = ecef.Y - ecefRef.Y, Z = ecef.Z - ecefRef.Z };

        double latRad = refLat * deg2rad;
        double lonRad = refLon * deg2rad;

        double[,] rotMat = new double[3, 3]
        {
            { -Math.Sin(lonRad), Math.Cos(lonRad), 0 },
            { -Math.Sin(latRad) * Math.Cos(lonRad), -Math.Sin(latRad) * Math.Sin(lonRad), Math.Cos(latRad)},
            { Math.Cos(latRad) * Math.Cos(lonRad), Math.Cos(latRad) * Math.Sin(lonRad), Math.Sin(latRad) }
        };


        Point3 enu = new Point3()
        {
            X = rotMat[0, 0] * diff.X + rotMat[0, 1] * diff.Y + rotMat[0, 2] * diff.Z,
            Y = rotMat[1, 0] * diff.X + rotMat[1, 1] * diff.Y + rotMat[1, 2] * diff.Z,
            Z = rotMat[2, 0] * diff.X + rotMat[2, 1] * diff.Y + rotMat[2, 2] * diff.Z,
        };

        return enu;
    }

    /// <summary>
	/// Converts from ECEF (Earth-centered Earth-fixed, meters) coordinates to ENU (East North Up, meters) coordinates, relative to the reference point. Assumes (0,0,0) reference ECEF
	/// </summary>
	/// <param name="ecef">Point in ECEF coordinates</param>
	/// <param name="refLat">Reference point latitude, degrees</param>
	/// <param name="refLon">Reference point longitude, degrees</param>
	/// <returns>Point in ENU coordinates</returns>
    public static Point3 ECEF_TO_ENU(Point3 ecef, double refLat, double refLon)
    {
        return ECEF_TO_ENU(ecef, refLat, refLon, new Point3 { X = 0, Y = 0, Z = 0 });
    }

    /// <summary>
	/// Converts from ENU (East North Up, meters) coordinates to ECEF (Earth-centered Earth-fixed, meters) coordinates, relative to the reference point
	/// </summary>
	/// <param name="enu">Point in ENU coordinates</param>
	/// <param name="refLat">Reference point latitude, degrees</param>
	/// <param name="refLon">Reference point longitude, degrees</param>
	/// <param name="ecefRef">Reference point ECEF coordinates</param>
	/// <returns>Point in ECEF coordinates</returns>
    public static Point3 ENU_TO_ECEF(Point3 enu, double refLat, double refLon, Point3 ecefRef)
    {
        double latRad = refLat * deg2rad;
        double lonRad = refLon * deg2rad;

        double[,] rotMat = new double[3, 3]
        {
            { -Math.Sin(lonRad),  -Math.Sin(latRad) * Math.Cos(lonRad), Math.Cos(latRad) * Math.Cos(lonRad) },
            { Math.Cos(lonRad), -Math.Sin(latRad) * Math.Sin(lonRad), Math.Cos(latRad) * Math.Sin(lonRad)},
            { 0, Math.Cos(latRad), Math.Sin(latRad) }
        };

        Point3 ecef = new Point3()
        {
            X = rotMat[0, 0] * enu.X + rotMat[0, 1] * enu.Y + rotMat[0, 2] * enu.Z + ecefRef.X, 
            Y = rotMat[1, 0] * enu.X + rotMat[1, 1] * enu.Y + rotMat[1, 2] * enu.Z + ecefRef.Y,
            Z = rotMat[2, 0] * enu.X + rotMat[2, 1] * enu.Y + rotMat[2, 2] * enu.Z + ecefRef.Z,
        };

        return ecef;
    }

    /// <summary>
	/// Converts from ENU (East North Up, meters) coordinates to ECEF (Earth-centered Earth-fixed, meters) coordinates, relative to the reference point. Assumes (0,0,0) reference ECEF
	/// </summary>
	/// <param name="enu">Point in ENU coordinates</param>
	/// <param name="refLat">Reference point latitude, degrees</param>
	/// <param name="refLon">Reference point longitude, degrees</param>
	/// <returns>Point in ECEF coordinates</returns>
    public static Point3 ENU_TO_ECEF(Point3 enu, double refLat, double refLon)
    {
        return ENU_TO_ECEF(enu, refLat, refLon, new Point3 { X = 0, Y = 0, Z = 0 });
    }
}