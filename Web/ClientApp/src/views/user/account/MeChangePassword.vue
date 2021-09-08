<template>
  <v-dialog v-model="dialog" width="unset" transition="fade-transition">
    <template #activator="{ on }">
      <v-btn text color="primary" v-on="on">
        Change Password
      </v-btn>
    </template>
    <v-card width="500px" :max-width="$vuetify.breakpoint.width - 40">
      <v-card-title>Change password</v-card-title>
      <v-card-text>
        <v-text-field
          label="Current Password"
          type="password"
          v-model="form.password"
          :error-messages="errors.password"
          persistent-placeholder
        >
        </v-text-field>
        <v-text-field
          label="New Password"
          type="password"
          v-model="form.newPassword"
          :error-messages="errors.newPassword"
          persistent-placeholder
        >
        </v-text-field>
        <v-text-field
          label="Confirm New Password"
          type="password"
          v-model="form.confirmNewPassword"
          :error-messages="errors.confirmNewPassword"
          persistent-placeholder
        >
        </v-text-field>
      </v-card-text>
      <v-card-actions>
        <v-btn
          pt-0
          class="mr-4"
          @click="submit"
          color="primary"
          :loading="loading"
          text
        >
          Ok
        </v-btn>
        <v-btn @click="close" text>
          Back
        </v-btn>
        <router-link :to="{ name: 'Forgot' }" class="body-2 d-block ml-3">Forgot your password?
        </router-link>
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

export default defineComponent({
  name: 'MeChangePassword',
  components: { EditorInput, AutoCompleteSelect },
  props: {
    user: {
      type: Object as PropType<UserBase>,
      required: true,
    },
  },
  setup(props, { emit }) {
    const dialog = ref(false);
    const { form, errors, setErrors, clearErrors, clearForm } = useForm({
      password: '',
      newPassword: '',
      confirmNewPassword: '',
    });

    const loading = ref(false);
    const http = useHttp();
    const { notify } = useMessage();

    async function submit() {
      loading.value = true;
      if (form.newPassword !== form.confirmNewPassword) {
        setErrors({
          confirmNewPassword: 'Password confirmation not match',
          newPassword: 'Password confirmation not match',
        });
        return;
      }
      try {
        await http.post('me/change-password', form);
        close();
        emit('change');
        notify({ text: 'Password changed successfully', type: 'success' });
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
