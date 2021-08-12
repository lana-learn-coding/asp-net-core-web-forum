import axios, { AxiosInstance, AxiosPromise, AxiosRequestConfig } from 'axios';
import { useRouter } from '@/composable/compat';

const http = axios.create({
  baseURL: process.env.VUE_APP_API_URI || '/api',
  withCredentials: true,
});

export interface HttpClient {
  unwrapped: AxiosInstance,

  request<T>(config: AxiosRequestConfig): Promise<T>;

  get<T>(url: string, config?: AxiosRequestConfig): Promise<T>;

  delete<T>(url: string, config?: AxiosRequestConfig): Promise<T>;

  post<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T>;

  put<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T>;
}

export function useHttp(): HttpClient {
  const router = useRouter();

  async function request<T>(promise: Promise<AxiosPromise<T>>): Promise<T> {
    try {
      return (await promise).data;
    } catch (e) {
      if (e.response.status === 401) {
        await router.push({ name: 'Unauthorized' });
      }
      throw e;
    }
  }

  return {
    unwrapped: http,
    async request<T>(config: AxiosRequestConfig): Promise<T> {
      return request(http.request(config));
    },
    async get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
      return request(http.get(url, config));
    },
    async delete<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
      return request(http.delete(url, config));
    },
    async post<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
      return request(http.post(url, data, config));
    },
    async put<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
      return request(http.put(url, data, config));
    },
  };
}
