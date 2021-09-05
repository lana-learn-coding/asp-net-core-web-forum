<template>
  <v-autocomplete
    :value="value"
    @input="x => $emit('input', x)"
    :items="data"
    :loading="loading"
    :search-input.sync="search"
    :error-messages="errorMessages"
    :item-value="itemValue"
    hide-no-data
    :item-text="itemText"
    :label="label"
    :persistent-placeholder="persistentPlaceholder"
    :required="required"
    :single-line="singleLine"
    :hide-details="hideDetails"
    :clearable="!required"
    :rules="required ? [ruleRequired] : []"
    @click:clear="$emit('input', '')"
  >
  </v-autocomplete>
</template>

<script lang="ts">
import { defineComponent, ref, watch } from '@vue/composition-api';
import { debounce } from 'vue-debounce';
import { useHttp } from '@/services/http';

export default defineComponent({
  name: 'AutoCompleteSelect',
  props: {
    value: String,
    uri: {
      type: String,
      required: true,
    },
    itemValue: {
      type: String,
      default: 'uid',
    },
    itemText: {
      type: String,
      default: 'text',
    },
    errorMessages: [String, Array],
    label: String,
    required: Boolean,
    persistentPlaceholder: Boolean,
    singleLine: Boolean,
    hideDetails: Boolean,
  },
  setup(props) {
    const search = ref('');
    const data = ref([]);
    const loading = ref(false);

    function ruleRequired(val: string): boolean | string {
      return !!val.trim() || 'Please select a value';
    }

    const http = useHttp();
    watch(search, debounce(async (val) => {
      try {
        data.value = await http.get(props.uri, { params: { search: val, limit: 10 } });
      } catch (e) {
        data.value = [];
      }
    }, 350));

    return {
      data,
      loading,
      search,
      ruleRequired,
    };
  },
});
</script>
