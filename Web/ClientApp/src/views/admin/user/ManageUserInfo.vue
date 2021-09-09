<template>
  <crud-table
    :table="table"
    :form="form"
    title="User Infos"
    url="admin/users/infos"
    :filter="filter"
    form-width="900px"
    :operations="['update']"
  >
    <template #filter="{bind, on}">
      <v-text-field
        class="mr-3"
        v-debounce="on.search"
        :value="bind.search"
        append-icon="search"
        label="Search"
        single-line
        hide-details
      >
      </v-text-field>
      <v-spacer class="d-none d-md-block"></v-spacer>
    </template>

    <template #form="{values, inputs, errors}">
      <v-row>
        <v-col class="text-subtitle-1 pb-0 text--primary font-weight-medium" cols="12">Personal
          Info
        </v-col>
        <v-col cols="6">
          <v-text-field
            :value="values.firstName"
            :error-messages="errors.firstName"
            @input="inputs.firstName"
            label="First Name"
            persistent-placeholder
          >
          </v-text-field>
        </v-col>
        <v-col cols="6">
          <v-text-field
            :value="values.lastName"
            :error-messages="errors.lastName"
            @input="inputs.lastName"
            label="Last Name"
            persistent-placeholder
          >
          </v-text-field>
        </v-col>
        <v-col cols="6">
          <v-text-field
            :value="values.phone"
            :error-messages="errors.phone"
            @input="inputs.phone"
            label="Phone"
            persistent-placeholder
          >
          </v-text-field>
        </v-col>
        <v-col cols="6">
          <gender-select
            :value="values.gender"
            :error-messages="errors.gender"
            @input="inputs.gender"
            persistent-placeholder
          >
          </gender-select>
        </v-col>
        <v-col cols="12" class="pb-0">
          <v-textarea
            outlined
            :value="values.bio"
            :error-messages="errors.bio"
            @input="inputs.bio"
            label="Bio"
            persistent-placeholder
          >
          </v-textarea>
        </v-col>
        <v-col class="py-0">
          <v-divider></v-divider>
        </v-col>
        <v-col class="text-subtitle-1 pb-0 text--primary font-weight-medium" cols="12">Work Info
        </v-col>
        <v-col cols="6">
          <auto-complete-select
            uri="regions/countries/all"
            :value="values.workCountryId"
            :error-messages="errors.workCountryId"
            @input="inputs.workCountryId"
            label="Country"
            item-value="uid"
            item-text="name"
            persistent-placeholder
          >
          </auto-complete-select>
        </v-col>
        <v-col cols="6">
          <auto-complete-select
            uri="regions/cities/all"
            :value="values.workCityId"
            :error-messages="errors.workCityId"
            @input="inputs.workCityId"
            label="City"
            item-value="uid"
            item-text="name"
            persistent-placeholder
          >
          </auto-complete-select>
        </v-col>
        <v-col cols="6">
          <v-text-field
            :value="values.workPhone"
            :error-messages="errors.workPhone"
            @input="inputs.workPhone"
            label="Work Phone"
            persistent-placeholder
          >
          </v-text-field>
        </v-col>
        <v-col cols="6">
          <v-text-field
            :value="values.workAddress"
            :error-messages="errors.workAddress"
            @input="inputs.workAddress"
            label="Work Address"
            persistent-placeholder
          >
          </v-text-field>
        </v-col>
        <v-col cols="6">
          <auto-complete-select
            uri="exp/positions/all"
            :value="values.workPositionId"
            :error-messages="errors.workPositionId"
            @input="inputs.workPositionId"
            label="Work Position"
            item-value="uid"
            item-text="name"
            persistent-placeholder
          >
          </auto-complete-select>
        </v-col>
        <v-col cols="6">
          <auto-complete-select
            uri="exp/experiences/all"
            :value="values.workExperienceId"
            :error-messages="errors.workExperienceId"
            @input="inputs.workExperienceId"
            label="Work Experience"
            item-value="uid"
            item-text="measurement"
            persistent-placeholder
          >
          </auto-complete-select>
        </v-col>
        <v-col cols="12">
          <auto-complete-select
            uri="specialties/all"
            :value="values.workSpecialitiesIds"
            :error-messages="errors.workSpecialitiesIds"
            @input="inputs.workSpecialitiesIds"
            label="Work specialities"
            item-value="uid"
            item-text="name"
            persistent-placeholder
            multiple
          >
          </auto-complete-select>
        </v-col>
        <v-col cols="12">
          <v-textarea
            outlined
            :value="values.workDescription"
            :error-messages="errors.workDescription"
            @input="inputs.workDescription"
            label="Work Description"
            persistent-placeholder
          >
          </v-textarea>
        </v-col>
      </v-row>
    </template>

    <template #table.user.avatar="{ item }">
      <v-avatar size="32">
        <v-img
          :src="item.avatar || '@/assets/anon_.png'"
          lazy-src="@/assets/anon_thumbnail.png">
        </v-img>
      </v-avatar>
    </template>

    <template #table.user.email="{ item }">
      {{ item.email }}
    </template>

    <template #table.user.username="{ item }">
      {{ item.username }}
    </template>

    <template #table.workExperience.level="{ item }">
      {{ item.workExperience ? item.workExperience.measurement : '' }}
    </template>

    <template #table.workPosition.name="{ item }">
      {{ item.workPosition ? item.workPosition.name : '' }}
    </template>

    <template #table.workPosition.name="{ item }">
      {{ item.workPosition ? item.workPosition.name : '' }}
    </template>

    <template #table.workCountry.name="{ item }">
      {{ item.workCountry ? item.workCountry.name : '' }}
    </template>

    <template #table.workCity.name="{ item }">
      {{ item.workCity ? item.workCity.name : '' }}
    </template>

    <template #table.updatedAt="{ item }">
      {{ formatDateTime(item.updatedAt) }}
    </template>
  </crud-table>
</template>

<script lang="ts">
import { defineComponent, reactive } from '@vue/composition-api';
import { formatDateTime } from '@/composable/date';
import CrudTable from '@/components/CrudTable.vue';
import CrudEditForm from '@/components/CrudEditForm.vue';
import EditorInput from '@/components/form/EditorInput.vue';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';
import GenderSelect from '@/components/form/GenderSelect.vue';

export default defineComponent({
  name: 'ManageUser',
  components: {
    GenderSelect,
    AutoCompleteSelect,
    EditorInput,
    CrudEditForm,
    CrudTable,
  },
  setup() {
    const table = [
      {
        text: 'Id',
        sortable: false,
        value: 'slug',
      },
      { text: 'Avatar', value: 'user.avatar' },
      { text: 'Username', value: 'user.username' },
      { text: 'Email', value: 'user.email' },
      { text: 'First Name', value: 'firstName' },
      { text: 'Last Name', value: 'lastName' },
      { text: 'Phone', value: 'phone' },
      { text: 'Work Phone', value: 'workPhone' },
      { text: 'Experience', value: 'workExperience.level' },
      { text: 'Position', value: 'workPosition.name' },
      { text: 'City', value: 'workCity.name' },
      { text: 'Country', value: 'workCountry.name' },
      { text: 'Updated At', value: 'updatedAt' },
      { text: 'Action', value: 'action', sortable: false },
    ];

    const filter = reactive({
      search: '',
    });

    const form = reactive({
      firstName: '',
      lastName: '',
      phone: '',
      bio: '',
      workPhone: '',
      workExperienceId: '',
      workPositionId: '',
      workAddress: '',
      workCityId: '',
      workCountryId: '',
      workDescription: '',
      workSpecialitiesIds: [],
      gender: null,
    });

    return {
      table,
      filter,
      formatDateTime,
      form,
    };
  },
});
</script>
