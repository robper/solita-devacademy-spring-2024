// See https://kit.svelte.dev/docs/types#app
// for information about these interfaces
declare global {
	namespace App {
		// interface Error {}
		// interface Locals {}
		// interface PageData {}
		// Extends eller implements min typ
		interface PageState {
			selected: SingleStationData;
		}
		// interface Platform {}
	}
}

export { };
