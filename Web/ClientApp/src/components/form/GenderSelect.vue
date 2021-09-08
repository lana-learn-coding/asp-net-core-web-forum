<template>
  <v-select
    :value="value"
    @input="input"
    :items="data"
    :error-messages="errorMessages"
    :label="label || 'Gender'"
    :persistent-placeholder="persistentPlaceholder"
    :required="required"
    :single-line="singleLine"
    :hide-details="hideDetails"
    :clearable="!required && !readonly"
    :rules="required ? [ruleRequired] : []"
    @click:clear="input('')"
    :readonly="readonly"
    :dense="dense"
  >
  </v-select>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';

export default defineComponent({
  name: 'GenderSelect',
  props: {
    value: Boolean,
    errorMessages: [String, Array],
    label: String,
    required: Boolean,
    persistentPlaceholder: Boolean,
    singleLine: Boolean,
    hideDetails: Boolean,
    readonly: Boolean,
    dense: Boolean,
  },
  setup(props, { emit }) {
    const data = [
      { text: 'Male', value: true },
      { text: 'Female', value: false },
    ];

    function input(val: string) {
      emit('input', val);
    }

    function ruleRequired(val: string): boolean | string {
      return !!val.trim() || 'Please select genders';
    }

    return {
      data,
      input,
      ruleRequired,
    };
  },
});
</script>
