<script lang="ts">
    import { onMount } from 'svelte';
    import { login, authStore } from '../../stores/authStore'; // A store to track login state

    
    let user: any;
    $: authStore.subscribe(value => user = value);

    let username = '';
    let password = '';
    let errorMessage = '';

    login(username, password);

    async function handleLogin() {
        errorMessage = '';

        try {
            login(username, password);
        } catch (error: any) {
            errorMessage = error.message;
        }
    }
</script>

{#if !user}
<form on:submit|preventDefault={handleLogin}>
    <label>
        Username:
        <input type="text" bind:value={username} required />
    </label>

    <label>
        Password:
        <input type="password" bind:value={password} required />
    </label>

    {#if errorMessage}
        <p class="error">{errorMessage}</p>
    {/if}

    <button type="submit">Login</button>
</form>

{:else}
    <p>Welcome, {user.username}!</p>
{/if}

<style>
    .error { color: red; }
</style>