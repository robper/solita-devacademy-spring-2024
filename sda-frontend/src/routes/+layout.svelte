<nav id="nav">
	<a href="/">Home</a>
	<a href="/stations">Stations</a>
</nav>
<div id="remaining">
	<slot />
</div>

<style>
	#nav {
		height: 80px;
		background-color: rgb(0, 255, 255);
	}

	#remaining {
		background-color: #6a9cff;
		height: calc(100vh - 80px);
		width: 100vw;
	}
</style>
