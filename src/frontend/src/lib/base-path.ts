import { dev } from '$app/environment';
import { base } from '$app/paths';

export const getApiPath = (): string => {
	if (dev) return 'http://localhost:5199';

	return window?.location.origin + base;
};

export const getAppPath = (): string => {
	return window?.location.origin + base;
};
