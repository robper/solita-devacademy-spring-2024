import {
    Marker,
    type MarkerOptions,
    type LatLngExpression,
} from "leaflet";
// Extend the Marker class to hold which station it represents
// This way we don't have to have a dictionary to translate them between layer id:station id
export class StationMarker extends Marker {
    stationId: Number | undefined;
    stationName: String | undefined;
    constructor(latLng: LatLngExpression, options?: MarkerOptions) {
        super(latLng, options);
    }
    setStationId(id: Number): this {
        this.stationId = id;
        return this;
    }
    setStationName(name: String): this {
        this.stationName = name;
        return this;
    }
}