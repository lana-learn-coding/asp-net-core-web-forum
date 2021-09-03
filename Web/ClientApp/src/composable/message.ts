import Vue from 'vue';

export const alerts = Vue.observable<{ value: AlertOptions[] }>({
  value: [],
});

export interface AlertOptions {
  width?: string;
  ok?: string;
  title?: string;
  subtitle?: string;
  text: string;
  cancel?: string | boolean;
}

export interface UseAlertResult {
  alert(opt: AlertOptions): Promise<void>;

  confirm(opt: AlertOptions): Promise<boolean>;
}

export function useAlert(): UseAlertResult {
  function alert(opt: AlertOptions): Promise<void> {
    opt.cancel = false;
    alerts.value = [...alerts.value, opt];
    return new Promise<void>((resolve) => {
      (opt as unknown as { cb: () => void }).cb = resolve;
    });
  }

  function confirm(opt: AlertOptions): Promise<boolean> {
    alerts.value = [...alerts.value, opt];
    return new Promise<boolean>((resolve) => {
      (opt as unknown as { cb: (val: boolean) => void }).cb = resolve;
    });
  }

  return {
    alert,
    confirm,
  };
}
