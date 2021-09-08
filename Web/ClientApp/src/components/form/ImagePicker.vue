<template>
  <v-file-input
    autofocus
    accept="image/*"
    prepend-icon=""
    show-size
    v-model="image"
    @change="uploadImage"
    :loading="loading"
    :error-messages="errorMessages"
    :label="label || 'Image'"
    :persistent-placeholder="persistentPlaceholder"
    :single-line="singleLine"
    :hide-details="hideDetails"
  >
    <template #selection>
      <div class="text-truncate" style="max-width: 250px">{{ input }}</div>
    </template>
  </v-file-input>
</template>

<script>
/* eslint-disable */
import { computed, defineComponent, ref } from '@vue/composition-api';
import { useHttp } from '@/services/http';

export default defineComponent({
  name: 'ImagePicker',
  props: {
    value: String,
    errorMessages: [String, Array],
    label: String,
    required: Boolean,
    persistentPlaceholder: Boolean,
    singleLine: Boolean,
    hideDetails: Boolean,
  },
  setup(props, { emit }) {
    const http = useHttp();
    const image = ref(null);
    const loading = ref(false);
    const input = computed({
      get: () => props.value,
      set: (val) => {
        emit('input', val);
      },
    });

    async function uploadImage() {
      loading.value = true;
      if (image.value) {
        const formData = new FormData();
        formData.append('upload', image.value);
        try {
          const imageData = await http.post('/data/images', formData, {
            headers: {
              'Content-Type': 'multipart/form-data',
            },
          });
          input.value = imageData.url;
        } catch (e) {
          alert('Cannot upload image');
        }
      }
      loading.value = false;
    }

    return {
      uploadImage,
      loading,
      image,
      input,
    };
  },
});
</script>
