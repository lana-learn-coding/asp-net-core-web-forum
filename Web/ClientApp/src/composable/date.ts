import dayjs from 'dayjs';

export function formatDate(date: dayjs.ConfigType, template = 'YYYY-MM-DD'): string {
  if (!date) {
    return '';
  }
  return dayjs(date)
    .format(template);
}

export function useDate(date: dayjs.ConfigType): dayjs.Dayjs {
  return dayjs(date);
}
