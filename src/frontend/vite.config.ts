import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vite';

export default defineConfig({
	plugins: [sveltekit()],
	server: {
		port: 5198,
		proxy: {
			'/api': {
				target: 'http://localhost:5199',
				cookiePathRewrite: {
					'*': '/'
				}
			}
		}
	}
});
