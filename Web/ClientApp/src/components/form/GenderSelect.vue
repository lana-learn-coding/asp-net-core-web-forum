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
      { text: 'Unknown', value: null },
    ];

    function input(val: string) {
      emit('input', val);
    }

    return {
      data,
      input,
    };
  },
});
</script>
