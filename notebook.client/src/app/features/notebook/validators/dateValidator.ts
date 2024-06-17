import { formatDate } from '@angular/common';
import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function dateValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    var urserDate = control.value;

    const format = 'dd/MM/yyyy';
    const currentDate = (Date.now(), format);

    console.log(urserDate);
    console.log(currentDate);

    if (urserDate > currentDate) {
      return null;
    }

    return { dateStrength: true };
  };
}
