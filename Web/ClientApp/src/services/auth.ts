import { useLocalStorage } from '@vueuse/core';
import { reactive } from '@vue/composition-api';
import { AxiosError } from 'axios';
import { useCoreHttp } from '@/services/http';
import { next, noop } from '@/composable/compat';

export interface UseUserResult {
  user: AuthUser;

  refresh(): Promise<boolean>;

  login(user: { username: string, password: string, rememberMe?: boolean }): Promise<void>;

  logout(): Promise<void>;
}

export interface AuthUser {
  uid: string;
  slug: string;
  username: string;
  email: string;
  avatar: string;

  roles: string[] | null;
  isAuthenticated: boolean;
  loading: boolean;
}

export interface JwtToken {
  token: string,
  user: AuthUser;
}

const defaultUser = {
  uid: '',
  slug: '',
  username: 'anon',
  email: 'anon',
  avatar: '',
  roles: null,
  isAuthenticated: false,
  loading: false,
};
const storage = useLocalStorage<AuthUser>('drforum-user', { ...defaultUser });
const config = {
  withCredentials: true,
  headers: { RefreshToken: true }, // To avoid repeat when retry 401
};

export function useUser(): UseUserResult {
  // use core axios instead of wrapped version
  // that way make this function independent from vue instance
  const { client } = useCoreHttp();
  const user = reactive(storage.value);

  // register auto-token refresh interceptor
  client.interceptors.response.use(next, async (error: AxiosError) => {
    if (!error.response?.status || error.response.status !== 401) throw error;
    if (error.request.headers.RefreshToken) throw error;
    if (!await refresh()) throw error; // cannot refresh

    error.config.headers.Authorization = client.defaults.headers.Authorization;
    return client.request(error.config);
  });

  async function login(form: { username: string, password: string, rememberMe: boolean }) {
    user.loading = true;
    try {
      const res = await client.post<JwtToken>('auth/login', form);
      Object.assign(user, res.data.user);
      client.defaults.headers.Authorization = `Bearer ${res.data.token}`;
      user.isAuthenticated = true;
    } finally {
      user.loading = false;
    }
  }

  async function logout() {
    user.loading = true;
    try {
      delete client.defaults.headers.Authorization;
      Object.assign(user, { ...defaultUser });
      await client.post('auth/login', null, config).catch(noop);
    } finally {
      user.loading = false;
    }
  }

  async function refresh() {
    try {
      const res = await client.post<JwtToken>('auth/refresh-token', null, config);
      Object.assign(user, res.data.user);
      client.defaults.headers.Authorization = `Bearer ${res.data.token}`;
      user.isAuthenticated = true;
      return true;
    } catch {
      Object.assign(user, { ...defaultUser });
      delete client.defaults.headers.Authorization;
      return false;
    }
  }

  return {
    user,
    login,
    logout,
    refresh,
  };
}
