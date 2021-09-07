import axios, { AxiosInstance, AxiosPromise, AxiosRequestConfig } from 'axios';
import {
  onMounted,
  reactive,
  Ref,
  ref,
  UnwrapRef,
  UnwrapRefSimple,
  watch,
} from '@vue/composition-api';
import { noop, useRoute, useRouter } from '@/composable/compat';
import { Dictionary, FlatDictionary, Page, PageMeta, Primitive } from '@/services/model';
import { useMessage } from '@/composable/message';

const http = axios.create({
  baseURL: process.env.VUE_APP_BASE_API_URL || '/api',
  withCredentials: true,
});

export class HttpClient {
  public readonly client: AxiosInstance = http;

  protected async intercept<T>(promise: Promise<AxiosPromise<T>>): Promise<T> {
    return (await promise).data;
  }

  async request<T>(config: AxiosRequestConfig): Promise<T> {
    return this.intercept(this.client.request(config));
  }

  async get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
    return this.intercept(this.client.get(url, config));
  }

  async delete<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
    return this.intercept(this.client.delete(url, config));
  }

  async post<T>(url: string, data?: Dictionary, config?: AxiosRequestConfig): Promise<T> {
    return this.intercept(this.client.post(url, data, config));
  }

  async put<T>(url: string, data?: Dictionary, config?: AxiosRequestConfig): Promise<T> {
    return this.intercept(this.client.put(url, data, config));
  }
}

export class HookedHttpClient extends HttpClient {
  private readonly router = useRouter();

  private readonly notify;

  private readonly confirm;

  constructor() {
    super();
    const { notify, confirm } = useMessage();
    this.notify = notify;
    this.confirm = confirm;
  }

  protected async intercept<T>(promise: Promise<AxiosPromise<T>>): Promise<T> {
    try {
      return await super.intercept(promise);
    } catch (e) {
      console.log(e.response.status);
      if (e.response.status === 401) {
        this.confirm({
          title: 'Login',
          text: 'Please login to continue',
          cancel: 'Home',
          persistent: true,
        })
          .then((ok) => {
            if (ok) {
              this.router.push({ name: 'Login' }).catch(noop);
              return;
            }
            this.router.push({ name: 'Home' });
          });
      } else if (e.response.status === 403) {
        await this.router.push({ name: 'Forbidden' });
        this.notify({ text: 'You don\'t have enough permission', type: 'warning' });
      } else if (e.response.status === 409) {
        if (e.response.data.message) {
          this.notify({ text: e.response.data.message, type: 'error' });
        }
      } else if (e.response.status === 404) {
        await this.router.push({ name: 'NotFound' });
      } else if (e.response.status.toString().startsWith('5')) {
        this.notify({ text: 'Operation failed', type: 'error' });
      }
      throw e;
    }
  }
}

export function useCoreHttp(): HttpClient {
  return new HttpClient();
}

export function useHttp(): HttpClient {
  return new HookedHttpClient();
}

type PrimitiveOrPrimitiveArray = Primitive | Primitive[]

export function useRouteQuery<T extends PrimitiveOrPrimitiveArray>(name: string, defaultValue?: T): Ref<UnwrapRef<T>> {
  if (defaultValue == null) {
    return useRouteQuery(name, '') as Ref<UnwrapRef<T>>;
  }

  const query = ref<T>(defaultValue);
  const route = useRoute();
  const router = useRouter();

  watch(
    query,
    (val) => {
      const queryParams = { ...route.query };
      const rawValue = val.toString().trim();
      if (!rawValue || rawValue === defaultValue.toString().trim()) {
        delete queryParams[name];
      } else {
        queryParams[name] = val as never;
      }
      router.push({ query: queryParams }).catch(noop);
    },
    { deep: true },
  );

  const queryParams = { ...route.query };
  if (queryParams[name]) {
    const converted = convertParamValue(queryParams[name], defaultValue);
    if (converted == null) {
      delete queryParams[name];
      router.push({ query: queryParams }).catch(noop);
    } else {
      query.value = converted as UnwrapRef<T>;
    }
  }
  return query;
}

export interface UseQueryResult<T, Q extends FlatDictionary> {
  data: Ref<UnwrapRef<T[]>>;
  meta: UnwrapRef<PageMeta & { loading: boolean; initialized: boolean }>;
  query: UnwrapRef<PagedQueryParam<Q>>;

  fetch(queryParams?: FlatDictionary, eager?: boolean): Promise<void>;
}

type PagedQueryParam<Q> = Q & { page: number; size: number }
type RouteQueryParam = string | (string | null)[] | null | undefined
type UseQueryCurlyFunction<T> = <Q extends FlatDictionary>(params?: Q) => UseQueryResult<T, Q>

export function useQuery<T>(url: string): UseQueryCurlyFunction<T> {
  return <Q extends FlatDictionary>(params?: Q) => {
    const data = ref<T[]>([]);
    const route = useRoute();

    const meta = reactive(
      {
        currentPage: 1,
        perPage: 15,
        totalPages: 1,
        totalItems: 0,
        loading: false,
        initialized: false,
      },
    );

    const baseQuery = { page: 1, size: 15, ...params };
    const query = reactive<PagedQueryParam<Q>>(mergeQuery(baseQuery, route.query) as PagedQueryParam<Q>);

    watch(
      query,
      (val) => fetch(val as FlatDictionary),
      { deep: true },
    );

    const router = useRouter();
    const client = useHttp();

    async function fetch(queryParams?: FlatDictionary, eager = true) {
      meta.loading = true;
      try {
        const newParams = { ...query, ...queryParams };
        const changed = Object.keys(newParams).filter((key) => newParams[key] !== query[key] && !key.startsWith('_'));

        // only fetch if query changed
        if (eager || changed.length > 0) {
          if (newParams.page && newParams.page === meta.currentPage) {
            newParams.page = 1;
          }

          console.log(newParams);
          const res = await client.get<Page<T>>(url, { params: newParams });
          data.value = res.data as UnwrapRefSimple<T>[];
          Object.assign(meta, res.meta);
          query.page = res.meta.currentPage;
          query.size = res.meta.perPage;
          newParams.page = res.meta.currentPage;
          newParams.size = res.meta.perPage;
        }

        const newQuery = { ...route.query, ...newParams };
        Object.keys(newQuery).forEach((key) => {
          const value = newQuery[key]?.toString().trim();
          if (value && value !== baseQuery[key]?.toString().trim()) {
            return;
          }
          delete newQuery[key];
        });
        await router.push({ query: newQuery as Record<string, RouteQueryParam> }).catch(noop);
      } finally {
        meta.loading = false;
      }
    }

    onMounted(async () => {
      await fetch();
      meta.initialized = true;
    });

    return {
      data,
      meta,
      query,
      fetch,
    };
  };
}

/**
 * Merge 2 query params to new ones
 * Work like Object.assign but with some improvement that help converting route queries
 * to typed query params
 *
 * @param origin origin params. this object maintain the origin type of each param
 * @param params the params to merge
 */
function mergeQuery(origin: FlatDictionary, params: Record<string, RouteQueryParam>): FlatDictionary {
  const result = { ...origin };
  if (!params) return result;

  Object.keys(params).forEach((param) => {
    const value = convertParamValue(params[param], result[param]);
    if (value === null) {
      delete result[param];
      return;
    }
    result[param] = value;
  });

  return result;
}

/**
 * Try to restore value type from example
 * ex:
 * value = '2', example = 1 => origin = 2
 * value = 2, example = 1 => origin = 2
 */
function convertParamValue<T extends PrimitiveOrPrimitiveArray>(raw: RouteQueryParam, example?: T): T | null {
  if (raw === null || raw === undefined) {
    return null;
  }

  if (example === null || example === undefined) {
    return raw as unknown as T; // unknown type of T as T is null
  }

  if (raw instanceof Array) {
    if (Array.isArray(example)) {
      if (example.length === 0) {
        return raw as unknown as T;
      }

      // If both is an array, then parse all the array using type of first item in example
      return raw.map((val) => convertParamValue(val, example[0])) as unknown as T;
    }

    // If example is not an array, then process first raw param
    if (raw.length === 0) raw.push('');
    return convertParamValue(raw[0], example);
  }

  // Convert if type is same
  if (typeof raw === typeof example) {
    return raw as unknown as T;
  }

  // Convert if type is different
  const value = raw?.toString().trim();
  if (!value) {
    return null;
  }

  if (typeof example === 'string') {
    return value as unknown as T;
  }

  if (typeof example === 'boolean') {
    return (value === 'true') as unknown as T;
  }

  if (typeof example === 'number') {
    const num = Number(value);
    if (!Number.isNaN(num)) {
      return num as unknown as T;
    }
  }
  return null;
}
