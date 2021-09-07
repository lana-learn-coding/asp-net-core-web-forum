<template>
  <div class="mt-10 d-flex justify-center">
    <v-card style="width: 500px">
      <v-card-title>Sign up</v-card-title>
      <v-card-subtitle>Create new Account</v-card-subtitle>
      <v-card-text>
        <v-form>
          <v-text-field
            v-model="form.email"
            :error-messages="errors.email"
            label="Email"
            persistent-placeholder
            required
          >
          </v-text-field>

          <v-text-field
            v-model="form.username"
            :error-messages="errors.username"
            label="Username"
            persistent-placeholder
            required
          >
          </v-text-field>

          <v-text-field
            v-model="form.password"
            :error-messages="errors.password"
            label="Password"
            type="password"
            persistent-placeholder
            required
          >
          </v-text-field>

          <v-text-field
            v-model="form.confirm_password"
            :error-messages="errors.confirm_password"
            label="Confirm Password"
            type="password"
            persistent-placeholder
            required
          >
          </v-text-field>

          <v-checkbox v-model="form.terms" :error-messages="errors.terms" dense>
            <template #label>
              <span class="body-2">I agree with</span>
              <router-link :to="{ name: 'Terms'}" class="ml-2 body-2">
                Dr. Forum's terms of use
              </router-link>
            </template>
          </v-checkbox>
        </v-form>
      </v-card-text>

      <v-card-actions class="pt-0">
        <v-btn color="primary" :loading="loading" @click="submit" text>
          Sign Up
        </v-btn>
        <v-btn @click="clear" text>Clear</v-btn>
        <v-btn :to="{ name: 'Login' }" text>Login</v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from '@vue/composition-api';
import { useRouter } from '@/composable/compat';
import { useHttp } from '@/services/http';
import { useForm } from '@/composable/form';
import { useMessage } from '@/composable/message';

export default defineComponent({
  name: 'SignUp',

  setup() {
    const { form, errors, setErrors, clearErrors, clearForm } = useForm({
      username: '',
      password: '',
      confirm_password: '',
      email: '',
      terms: false,
    });

    const loading = ref(false);
    const http = useHttp();
    const router = useRouter();
    const { confirm } = useMessage();

    async function submit() {
      if (form.password !== form.confirm_password) {
        setErrors({
          confirm_password: 'Password confirmation not match',
          password: 'Password confirmation not match',
        });
        return;
      }
      if (!form.terms) {
        setErrors({
          terms: 'Please read and agree with ours terms of use',
        });
        return;
      }
      loading.value = true;
      try {
        await http.post('/auth/sign-up', form);
        clearForm();
        loading.value = false;

        const ok = await confirm({
          title: 'Signed Up',
          text: 'Your account was created! Please login',
          type: 'success',
          cancel: 'Home',
        });
        if (ok) await router.push({ name: 'Login' });
        else await router.push({ name: 'Home' });
      } catch (e) {
        setErrors(e.response.data);
      } finally {
        loading.value = false;
      }
    }

    function clear() {
      clearErrors();
      clearForm();
    }

    return {
      loading,
      form,
      errors,
      clear,
      submit,
    };
  },
});
</script>
