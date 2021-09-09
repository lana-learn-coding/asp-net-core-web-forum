<template>
  <v-dialog v-model="dialog" width="unset" transition="fade-transition">
    <template #activator="{ on }">
      <v-btn text color="success" v-on="on">
        Change Avatar
      </v-btn>
    </template>
    <v-card width="350px" :max-width="$vuetify.breakpoint.width - 40">
      <v-card-title>Change Avatar</v-card-title>
      <v-card-subtitle>Click on input to change</v-card-subtitle>
      <v-card-text>
        <image-picker
          v-model="form.avatar"
          :error-messages="errors.avatar"
          persistent-placeholder
          label="Avatar"
        >
        </image-picker>
        <v-img
          :src="user.avatar || require('@/assets/anon.png')"
          lazy-src="@/assets/anon_thumbnail.png"
          :alt="user.username"
          rounded
        ></v-img>
      </v-card-text>
      <v-card-actions>
        <v-btn
          class="mr-4"
          @click="submit"
          color="primary"
          :loading="loading"
          :disabled="old === form.avatar"
        >
          Ok
        </v-btn>
        <v-btn @click="close" text>
          Back
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { defineComponent, PropType, ref } from '@vue/composition-api';
import { useHttp } from '@/services/http';
import { useForm } from '@/composable/form';
import { useMessage } from '@/composable/message';
import { UserBase } from '@/services/model';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';
import EditorInput from '@/components/form/EditorInput.vue';
import ImagePicker from '@/components/form/ImagePicker.vue';
import { useUser } from '@/services/auth';
import { noop } from '@/composable/compat';

export default defineComponent({
  name: 'MeChangeAvatar',
  components: { ImagePicker, EditorInput, AutoCompleteSelect },
  props: {
    user: {
      type: Object as PropType<UserBase>,
      required: true,
    },
  },
  setup(props, { emit }) {
    const dialog = ref(false);
    const old = ref(props.user.avatar);
    const { form, errors, setErrors, clearErrors, clearForm } = useForm({
      avatar: props.user.avatar,
    });

    const loading = ref(false);
    const http = useHttp();
    const { notify } = useMessage();
    const { refresh } = useUser();

    async function submit() {
      loading.value = true;
      try {
        await http.post('me/change-avatar', form);
        close();
        emit('change');
        await refresh().catch(noop);
        notify({ text: 'Avatar changed successfully', type: 'success' });
        old.value = props.user.avatar;
      } catch (e) {
        if (e.response.data) {
          setErrors(e.response.data);
          notify({ text: 'Please check the input values', type: 'warning' });
          return;
        }
      } finally {
        loading.value = false;
      }
    }

    function close() {
      clearErrors();
      clearForm();
      dialog.value = false;
    }

    return {
      old,
      form,
      errors,
      submit,
      close,
      loading,
      dialog,
    };
  },
});
</script>
