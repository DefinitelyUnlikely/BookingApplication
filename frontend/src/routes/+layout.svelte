<script lang="ts">
	import Navbar from '../components/navbar.svelte';

	let { children } = $props();
</script>

<Navbar />

<main>{@render children()}</main>

<style>
	main {
		display: flex;
		flex-direction: column;
		justify-content: flex-start;
		align-items: center;
		margin: 1rem;
	}
</style>
