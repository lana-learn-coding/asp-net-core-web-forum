import { reactive, Ref, ref, UnwrapRef } from '@vue/composition-api';
import { Category, Dictionary } from '@/services/model';
import { useCoreHttp } from '@/services/http';

export interface UseFormResult<T> {
  form: UnwrapRef<T>;
  errors: UnwrapRef<T>;

  clearForm();

  clearErrors();

  setErrors(errors: Dictionary);

  setForm(form: Dictionary);
}

export function useForm<T extends Dictionary>(data: T): UseFormResult<T> {
  const form = reactive<T>({ ...data });
  const errors = reactive<T>({ ...data });

  function clearForm() {
    Object.assign(form, data);
  }

  function clearErrors() {
    Object.keys(data).forEach((key) => {
      (errors as Dictionary)[key] = [];
    });
  }

  function setErrors(newErrors: Dictionary) {
    clearErrors();
    Object.assign(errors, newErrors);
  }

  function setForm(newForm: Dictionary) {
    clearForm();
    Object.assign(form, newForm);
  }

  clearErrors();
  return {
    form,
    errors,
    clearForm,
    clearErrors,
    setErrors,
    setForm,
  };
}

export interface UseListResult<T> {
  data: Ref<T[]>;

  fetch(): Promise<void>;

  loading: Ref<boolean>;
}

let categoriesCache: Category[] = [];

export function useCategories(): UseListResult<Category> {
  const data = ref([...categoriesCache]);
  const loading = ref(false);
  const client = useCoreHttp();

  async function fetch() {
    try {
      loading.value = true;
      const res = await client.get<Category[]>('/categories/all');
      categoriesCache = res.sort((a, b) => a.priority - b.priority);
      data.value = [...categoriesCache];
    } catch {
      if (!data.value.length) {
        data.value = [{
          uid: '00000000-0000-0000-0000-000000000000',
          slug: 'uncategorized',
          name: 'Uncategorized',
          priority: 9999,
          createdAt: '2021-08-30 00:00:00',
          updatedAt: '2021-08-30 00:00:00',
        }];
      }
    } finally {
      loading.value = false;
    }
  }

  if (categoriesCache.length === 0) {
    fetch();
  }

  return {
    data,
    fetch,
    loading,
  };
}

export function useAccessType(): { name: string, color: string }[] {
  return [
    { name: 'Public', color: 'green' },
    { name: 'Protected', color: 'yellow darken-2' },
    { name: 'Internal', color: 'orange' },
    { name: 'Private', color: 'deep-orange' },
  ];
}

const priorities = [
  { name: 'VeryHigh', color: 'green', max: 10 },
  { name: 'High', color: 'yellow darken-2', max: 20 },
  { name: 'Normal', color: 'orange', max: 30 },
  { name: 'Low', color: 'deep-orange', max: 40 },
];

export function usePriority(priority: number): { name: string, color: string } {
  if (priority >= 40) {
    return priorities[3];
  }
  return priorities.find((x) => x.max + 10 > priority) ?? priorities[3];
}

export function useThreadStatus(): { name: string, color: string }[] {
  return [
    { name: 'Approved', color: 'green' },
    { name: 'Pending', color: 'yellow darken-2' },
    { name: 'Rejected', color: 'orange' },
    { name: 'Closed', color: 'deep-orange' },
  ];
}
