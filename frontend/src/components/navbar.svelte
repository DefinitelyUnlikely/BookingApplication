<script lang="ts">
	import { Hamburger } from 'svelte-hamburgers';
	import { slide } from 'svelte/transition';

	let open = $state(false);

	function changeOpenState() {
		open = !open;
	}
</script>

<nav>
	<div class="left-nav"></div>
	<div class="middle-nav"><h1>Booking Application</h1></div>
	<div class="right-nav">
		<Hamburger bind:open title="Toogle a menu with navigation" --color="#6bbaecff" type="minus" />
	</div>
</nav>
{#if open}
	<div class="hamburger-dropdown" transition:slide>
		<h3><a href="/" onclick={changeOpenState}>Home</a></h3>
		<h3><a href="/about" onclick={changeOpenState}>About</a></h3>
		<h3><a href="/booking" onclick={changeOpenState}>Booking</a></h3>
	</div>
{/if}

<style>
	nav {
		display: grid;
		grid-template-columns: 2fr 8fr 2fr;
		background-color: #7880b5ff;
		width: 100vw;
		height: 4rem;
		gap: 0.5rem;
		flex-shrink: 0;
	}

	.middle-nav {
		display: flex;
		align-items: center;
		justify-content: center;
	}
	.middle-nav > h1 {
		color: #6bbaecff;
	}
	.right-nav {
		display: flex;
		justify-content: end;
		align-items: center;
	}

	.hamburger-dropdown {
		height: 40vh;
		background-color: #7880b5ff;
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
	}

	.hamburger-dropdown h3 {
		margin: 0.5rem;
	}

	.hamburger-dropdown a {
		text-decoration: none;
		color: white;
	}
</style>
