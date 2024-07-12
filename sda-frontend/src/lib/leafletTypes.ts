import {
    Marker,
    type MarkerOptions,
    type LatLngExpression,
} from "leaflet";
// Extend the Marker class to hold which station it represents
// This way we don't have to have a dictionary to translate them between layer id:station id
export class StationMarker extends Marker implements Station {
    constructor(latLng: LatLngExpression, options?: MarkerOptions) {
        super(latLng, options);
    }
    id: number = 1;
    station_name: string | undefined = "";
    station_address: string | undefined = '';
    coordinate_x: string | undefined = '';
    coordinate_y: string | undefined = '';
    setStationId(id: number): this {
        //this.stationId = id;
        this.id = id;
        return this;
    }
    setStationName(name: string): this {
        //this.stationName = name;
        this.station_name = name;
        return this;
    }
}