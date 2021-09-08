<template>
  <v-col cols="12" md="9">
    <v-card flat>
      <v-card-title>
        <span>Yours Profile</span>
        <v-spacer></v-spacer>
        <v-btn text color="error" @click="edit" v-if="readonly" :loading="loading">
          Edit <span class="d-none d-md-block ml-1">Profile</span>
        </v-btn>
        <v-btn text color="primary" @click="submit" v-if="!readonly" :loading="loading">
          Submit
        </v-btn>
        <v-btn text @click="cancel" v-if="!readonly" :loading="loading">
          Cancel
        </v-btn>
      </v-card-title>
      <v-card-subtitle>Manage your profile</v-card-subtitle>
      <v-card-text>
        <v-row>
          <v-col cols="12" lg="10">
            <v-row>
              <v-col cols="12" sm="6" lg="3">
                <v-text-field
                  v-model="form.firstName"
                  :error-messages="errors.lastName"
                  label="First Name"
                  placeholder="Your first name"
                  dense
                  persistent-placeholder
                  :readonly="readonly">
                </v-text-field>
              </v-col>
              <v-col cols="12" sm="6" lg="3">
                <v-text-field
                  v-model="form.lastName"
                  :error-messages="errors.lastName"
                  dense
                  label="Last Name"
                  placeholder="Your last name"
                  persistent-placeholder
                  :readonly="readonly"
                >
                </v-text-field>
              </v-col>
              <v-col cols="12" sm="6" lg="3">
                <v-text-field
                  v-model="form.email"
                  :error-messages="errors.email"
                  dense
                  label="Email"
                  placeholder="Your email address"
                  persistent-placeholder
                  disabled
                >
                </v-text-field>
              </v-col>
              <v-col cols="12" sm="6" lg="3">
                <v-text-field
                  v-model="form.phone"
                  :error-messages="errors.phone"
                  dense
                  label="Phone Number"
                  placeholder="Your phone number"
                  persistent-placeholder
                  :readonly="readonly"
                >
                </v-text-field>
              </v-col>
              <v-col cols="12" sm="8" lg="5" xl="6">
                <date-picker
                  v-model="form.dateOfBirth"
                  :error-messages="errors.dateOfBirth"
                  dense
                  label="Birthdate"
                  placeholder="Your Birthdate"
                  persistent-placeholder
                  :readonly="readonly"
                ></date-picker>
              </v-col>
              <v-col cols="12" sm="4" lg="3">
                <gender-select
                  v-model="form.gender"
                  :error-messages="errors.gender"
                  dense
                  label="Gender"
                  placeholder="Select Gender"
                  persistent-placeholder
                  :readonly="readonly"
                >
                </gender-select>
              </v-col>
              <v-col cols="12" lg="8" xl="9">
                <v-textarea
                  v-model="form.bio"
                  :error-messages="errors.bio"
                  dense
                  label="Bio"
                  placeholder="Write some thing about your self"
                  persistent-placeholder
                  rows="4"
                  outlined
                  :readonly="readonly"
                >
                </v-textarea>
              </v-col>
              <v-col cols="12" lg="8" class="d-flex flex-column flex-sm-row pt-0">
                <v-checkbox
                  class="mr-3 mr-lg-10"
                  v-model="form.showPhone"
                  :error-messages="errors.showPhone"
                  label=" Public your phone number"
                  :readonly="readonly"
                  dense>
                </v-checkbox>
                <v-checkbox
                  v-model="form.showEmail"
                  :error-messages="errors.showEmail"
                  label="Public your email address"
                  :readonly="readonly"
                  dense>
                </v-checkbox>
              </v-col>
            </v-row>
          </v-col>
        </v-row>
      </v-card-text>
      <v-card-title class="pt-0">Yours Experience</v-card-title>
      <v-card-text>
        <v-row>
          <v-col cols="12" lg="10">
            <v-row>
              <v-col cols="12" sm="6" lg="4">
                <auto-complete-select
                  uri="regions/countries/all"
                  v-model="form.workCountryId"
                  :error-messages="errors.workCountryId"
                  label="Work Country"
                  placeholder="Work Country"
                  item-value="uid"
                  item-text="name"
                  dense
                  persistent-placeholder
                  :readonly="readonly"
                >
                </auto-complete-select>
              </v-col>
              <v-col cols="12" sm="6" lg="5">
                <auto-complete-select
                  uri="regions/cities/all"
                  v-model="form.workCityId"
                  :error-messages="errors.workCityId"
                  label="Work City"
                  placeholder="Work City"
                  item-value="uid"
                  item-text="name"
                  dense
                  persistent-placeholder
                  :readonly="readonly"
                >
                </auto-complete-select>
              </v-col>
              <v-col cols="12" lg="5" xl="6">
                <v-text-field
                  v-model="form.workAddress"
                  :error-messages="errors.workAddress"
                  label="My work address"
                  placeholder="Your hospital/home address..."
                  persistent-placeholder
                  :readonly="readonly"
                  dense
                ></v-text-field>
              </v-col>
              <v-col cols="12" lg="4" xl="3">
                <v-text-field
                  v-model="form.workPhone"
                  :error-messages="errors.workPhone"
                  label="My work phone"
                  placeholder="Your work phone"
                  persistent-placeholder
                  :readonly="readonly"
                  dense
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6" lg="4">
                <auto-complete-select
                  uri="exp/positions/all"
                  v-model="form.workPositionId"
                  :error-messages="errors.workPositionId"
                  label="Work position"
                  placeholder="Your position"
                  item-value="uid"
                  item-text="name"
                  dense
                  persistent-placeholder
                  :readonly="readonly"
                >
                </auto-complete-select>
              </v-col>
              <v-col cols="12" sm="6" lg="5">
                <auto-complete-select
                  uri="exp/experiences/all"
                  v-model="form.workExperienceId"
                  :error-messages="errors.workExperienceId"
                  label="Years of work"
                  placeholder="Experiences / Years of work"
                  item-value="uid"
                  item-text="measurement"
                  dense
                  persistent-placeholder
                  :readonly="readonly"
                >
                </auto-complete-select>
              </v-col>
              <v-col cols="12" lg="8" xl="9">
                <auto-complete-select
                  uri="specialties/all"
                  v-model="form.workSpecialitiesIds"
                  :error-messages="errors.workSpecialitiesIds"
                  label="I have experience in"
                  placeholder="My Specialties"
                  item-value="uid"
                  item-text="name"
                  persistent-placeholder
                  :readonly="readonly"
                  dense
                  multiple
                >
                </auto-complete-select>
              </v-col>
              <v-col cols="12" lg="8" xl="9">
                <v-textarea
                  v-model="form.workDescription"
                  :error-messages="errors.workDescription"
                  dense
                  label="About my work"
                  placeholder="Write some thing about your work and experience"
                  persistent-placeholder
                  rows="3"
                  outlined
                  :readonly="readonly"
                >
                </v-textarea>
              </v-col>
              <v-col cols="12" lg="8" class="d-flex flex-column flex-sm-row pt-0">
                <v-checkbox
                  class="mr-3 mr-lg-8"
                  v-model="form.showWorkAddress"
                  :error-messages="errors.showWorkAddress"
                  label=" Public your work contact"
                  :readonly="readonly"
                  dense>
                </v-checkbox>
                <v-checkbox
                  class="mr-3 mr-lg-8"
                  v-model="form.showWorkExperience"
                  :error-messages="errors.showWorkExperience"
                  label="Public your work experience"
                  :readonly="readonly"
                  dense>
                </v-checkbox>
                <v-checkbox
                  v-model="form.showWorkSpecialities"
                  :error-messages="errors.showWorkSpecialities"
                  label="Public your specialities"
                  :readonly="readonly"
                  dense>
                </v-checkbox>
              </v-col>
            </v-row>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>

    <me-account :user="user"></me-account>
  </v-col>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import { useBreadcrumbs } from '@/composable/breadcrumbs';
import MeAccount from '@/views/user/account/MeAccount.vue';
import { useRequireAuth } from '@/services/auth';
import { useHttp } from '@/services/http';
import { useMessage } from '@/composable/message';
import { useForm } from '@/composable/form';
import GenderSelect from '@/components/form/GenderSelect.vue';
import DatePicker from '@/components/form/DatePicker.vue';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';

export default defineComponent({
  name: 'Me',
  components: { AutoCompleteSelect, DatePicker, GenderSelect, MeAccount },
  setup() {
    useTitle('Your account');
    useBreadcrumbs([
      { text: 'Home', name: 'Home' },
      { text: 'Me', disabled: true },
    ]);
    const loading = ref(false);
    const readonly = ref(true);
    const { user } = useRequireAuth('User');
    const { notify } = useMessage();
    const { form, errors, setErrors, clearErrors } = useForm({
      firstName: '',
      lastName: '',
      gender: '',
      bio: '',
      phone: '',
      dateOfBirth: '',
      showPhone: false,
      showEmail: true,
      showWorkAddress: true,
      showWorkExperience: true,
      showWorkSpecialities: true,
      workSpecialitiesIds: [],
      workDescription: '',
      workPhone: '',
      workAddress: '',
      workCityId: '',
      workCountryId: '',
      workExperienceId: '',
      workPositionId: '',
    });

    const http = useHttp();
    onMounted(fetch);

    async function fetch() {
      if (!user.isAuthenticated) return;
      try {
        const res = await http.get('me');
        Object.assign(form, res);
      } catch {
        notify({ text: 'Please reload page', type: 'error' });
      }
    }

    const { alert, confirm } = useMessage();

    async function edit() {
      await alert({ title: 'Edit enabled', text: 'You are now able to edit profile' });
      readonly.value = false;
    }

    async function submit() {
      const ok = await confirm({
        title: 'Submit',
        text: 'Are you sure want to update profile ?',
        type: 'warning',
      });
      if (!ok) return;
      loading.value = true;
      try {
        const res = await http.post('me/profile', form);
        Object.assign(form, res);
        notify({ text: 'Profile updated successfully', type: 'success' });
        readonly.value = true;
        clearErrors();
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

    async function cancel() {
      loading.value = true;
      try {
        await fetch();
        clearErrors();
        readonly.value = true;
      } catch (e) {
        notify({ text: 'Please reload page', type: 'warning' });
      } finally {
        notify({ text: 'Cancelled profile edit' });
        loading.value = false;
      }
    }

    return {
      cancel,
      submit,
      edit,
      loading,
      errors,
      form,
      user,
      readonly,
    };
  },
});
</script>
