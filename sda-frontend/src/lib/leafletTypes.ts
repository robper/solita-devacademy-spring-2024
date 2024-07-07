import L, {
    FeatureGroup,
    Polyline,
    Popup,
    type LatLngExpression,
} from "leaflet";
// Extend the Marker class to hold which station it represents
// This way we don't have to have a dictionary to translate them between layer id:station id
export class StationMarker extends L.Marker {
    stationId: Number | undefined;
    constructor(latLng: LatLngExpression, options?: L.MarkerOptions) {
        super(latLng, options);
    }
    setStationId(id: Number): this {
        this.stationId = id;
        return this;
    }
}