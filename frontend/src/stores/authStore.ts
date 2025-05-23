import { writable } from 'svelte/store';

export const authStore = writable(null); // Stores user session info

export async function login(username: string, password: string) {
    const response = await fetch('localstorage', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    });

    if (response.ok) {
        const data = await response.json();
        authStore.set(data.user); // Store user session
        localStorage.setItem('token', data.token); // Save token
    }
}