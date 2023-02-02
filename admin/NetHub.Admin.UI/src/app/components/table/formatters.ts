import {DateTime} from 'luxon';
import {getDomain} from "../../shared/utilities";

const maxStringLength = 28;

export function formatAsDate(value: string | undefined): string {
  return value ? DateTime.fromISO(value).toFormat('dd/MM/yyyy HH:mm') : '–';
}

export function formatCheckmark(value: string | undefined): string {
  if (value) {
    return '<span class="check check-true">yes</span>';
  } else {
    return '<span class="check check-false">no</span>';
  }
}

export function formatCounter(value: number){
  if (value > 0){
    return `<span class="counter counter-positive">💚 ${value}</span>`;
  }
  else if (value < 0){
    return `<span class="counter counter-negative">❤️ ${-value}</span>`;

  }
  else {
    return `<span class="counter">🤍 ${value}</span>`;
  }
}

export function formatLink(link: string | null) {

  if (link) {
    const display = getDomain(link);
    return `<a href=\"${link}\" title=\"${link}\">${display}</a>`;
  } else {
    return '-'
  }
}

export function formatAsText(value: string | undefined): string {
  if (!value) {
    return '–';
  }
  if (value.length > maxStringLength) {
    return value.substring(0, maxStringLength - 3) + '...';
  }
  return value;
}
