<nav>
	<div class="left">
		<a href="/">Home</a>
		<a href="/stations">Stations</a>
		<a href="/map">Map</a>
	</div>
	<div class="right">
		<a
			class="icon"
			href="https://github.com/robper/solita-devacademy-spring-2024"
			target="_blank"
		>
			<img src="/github-mark.png" alt="git" />
		</a>
	</div>
</nav>
<div id="content">
	<slot />
</div>

<style>
	#content {
		height: calc(100vh - 55px);
		width: 100vw;
		z-index: 1;
		position: relative;
	}
	nav {
		height: 55px;
		width: 100%;
		background-color: var(--color-foreground);
		display: flex;
		flex-direction: row;
		justify-content: flex-start;

		/* Shadow*/
		z-index: 10;
		position: relative;
		align-items: center;
		box-shadow: 0px 0px 4px 1px var(--color-shade);
	}
	nav .right {
		margin-left: auto;
		margin-right: 10px;
	}
	nav .left {
		display: flex;
		justify-content: flex-start;
		gap: 5px;
		margin-left: 10px;
	}
	nav a {
		border-radius: 5px;
		display: block;
		padding-left: 10px;
		padding-right: 10px;
		padding-top: 10px;
		padding-bottom: 10px;
		text-decoration: none;
	}
	nav a:hover {
		background-color: var(--color-popout);
	}
	/* Git logo*/
	img {
		height: 25px;
	}
</style>
